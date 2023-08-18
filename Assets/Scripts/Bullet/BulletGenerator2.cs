using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 追尾弾
/// </summary>
public class BulletGenerator2 : MonoBehaviour
{
    public int bulletLevel = 1;

    private Bullet2 bulletPrefab;

    private Transform temporaryObjectsPlace;

    private CharaController charaController;

    private EnemyController target;  //追尾対象の敵


    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpBulletGenerator2(CharaController charaController)
    {
        this.charaController = charaController;

        bulletPrefab = this.charaController.bullet2Prefab;

        temporaryObjectsPlace = this.charaController.temporaryObjectsPlace;
    }

    /// <summary>
    /// バレット生成の準備
    /// </summary>
    public IEnumerator PrepareGenerateBullet()
    {
        switch (bulletLevel)
        {
            case 1:
                yield return StartCoroutine(GenerateBullet());
                break;

            case 2:
                for (int i = 0; i < 2; i++)
                {
                    yield return StartCoroutine(GenerateBullet());
                }
                break;

            case 3:
                for (int i = 0; i < 3; i++)
                {
                    yield return StartCoroutine(GenerateBullet());
                }
                break;

            case 4:
                for (int i = 0; i < 4; i++)
                {
                    yield return StartCoroutine(GenerateBullet());
                }
                break;

            default:
                for (int i = 0; i < 5; i++)
                {
                    yield return StartCoroutine(GenerateBullet());
                }
                break;
        }
    }

    /// <summary>
    /// バレット生成
    /// </summary>
    private IEnumerator GenerateBullet()
    {
        //ターゲットを探す
        yield return StartCoroutine(FindNearestEnemy());

        //ターゲットが見つかった場合のみ、バレットを生成
        if (target)
        {
            Bullet2 bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            bullet.transform.SetParent(temporaryObjectsPlace);

            bullet.target = target;

            //bullet.FindNearestEnemy(charaController.Direction);

            yield return new WaitForSeconds(0.2f);
        }
    }

    /// <summary>
    /// 一番近い敵を見つける(それぞれ違う敵を追尾対象とする)
    /// </summary>
    /// <returns></returns>
    public IEnumerator FindNearestEnemy()
    {
        target = null;

        //敵が存在している場合のみ処理を行う
        if (GameData.instance.enemiesList.Count > 0)
        {
            //enemiesListの中身をOrderBy()で小さい順に並べ替える(距離順にソート)
            var sortedEnemies = GameData.instance.enemiesList.OrderBy(enemy => Vector2.Distance(transform.position, enemy.transform.position));

            foreach (EnemyController enemy in sortedEnemies)
            {
                //enemyがtargetListに含まれている場合はスキップ
                if (GameData.instance.targetList.Exists(existingTarget => existingTarget == enemy))
                {
                    continue;
                }

                target = enemy;

                break;  //最も近い敵が見つかったらループを終了
            }

            if (target)
            {
                //最終的なターゲットをリストに追加(追尾弾それぞれのターゲットが被らないようにする)
                GameData.instance.targetList.Add(target);
            }

            //Destroy(gameObject, destroyTime);
        }

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            //追尾対象リストから要素を削除
            GameData.instance.targetList.Remove(target);

            Destroy(gameObject);
        }
    }
}
