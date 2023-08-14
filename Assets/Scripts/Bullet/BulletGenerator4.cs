using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 雷
/// </summary>
public class BulletGenerator4 : MonoBehaviour
{
    [SerializeField] private Bullet4 bulletPrefab;

    [SerializeField] private Transform temporaryObjectsPlace;

    [SerializeField] private CharaController charaController;

    private EnemyController target;  //攻撃対象

    private Vector2 offsetPos;  //最終的な生成位置

    public float attackInterval;


    /// <summary>
    /// GenerateBulletを実行
    /// </summary>
    public void PrepareGenerateBullet()
    {
        StartCoroutine(GenerateBullet());
    }

    /// <summary>
    /// バレット生成
    /// </summary>
    private IEnumerator GenerateBullet()
    {
        yield return StartCoroutine(CalculateGeneratePos());

        //targetが存在している場合だけバレット生成
        if (target)
        {
            Bullet4 bullet = Instantiate(bulletPrefab, offsetPos, Quaternion.identity);

            bullet.transform.SetParent(temporaryObjectsPlace);

            Destroy(bullet.gameObject, bullet.destroyTime);

            //targetListからtargetを削除
            GameData.instance.targetList.Remove(target);
        }
    }

    /// <summary>
    /// 一番近い敵を見つけ、攻撃対象とする。ただし、すでにtargetListに入っている敵は攻撃対象としない
    /// </summary>
    //private IEnumerator FindNearestEnemy()
    //{
    //    target = null;

    //    var sortedEnemies = GameData.instance.enemiesList.OrderBy(enemy => Vector2.Distance(transform.position, enemy.transform.position));

    //    foreach (EnemyController enemy in sortedEnemies)
    //    {
    //        if (GameData.instance.targetList.Exists(existingTarget => existingTarget == enemy))
    //        {
    //            continue;
    //        }

    //        target = enemy;

    //        break;
    //    }

    //    if (target)
    //    {
    //        GameData.instance.targetList.Add(target);
    //    }

    //    yield return null;
    //}

    /// <summary>
    /// ランダムな敵を攻撃対象とする(すでにtargetListに入っている敵はターゲットとしない)
    /// </summary>
    /// <returns></returns>
    private IEnumerator FindTarget()
    {
        target = null;

        if (GameData.instance.enemiesList.Count > 0)
        {
            //do_while文 <= do{ 条件式がtrueの間繰り返される処理 }while(条件式)。この場合、targetListにtargetが含まれている間、do{}内の処理を繰り返す
            do
            {
                int randomNo = Random.Range(0, GameData.instance.enemiesList.Count);

                target = GameData.instance.enemiesList[randomNo];

            } while (GameData.instance.targetList.Contains(target));

            GameData.instance.targetList.Add(target);
        }

        yield return null;
    }

    /// <summary>
    /// バレットの生成位置を計算
    /// </summary>
    private IEnumerator CalculateGeneratePos()
    {
        //yield return StartCoroutine(FindNearestEnemy());

        yield return StartCoroutine(FindTarget());

        if (target)
        {
            offsetPos = new Vector2(target.transform.position.x, target.transform.position.y + 3.5f);
        }
    }

    /// <summary>
    /// レベルに応じて攻撃のインターバル時間を変える
    /// </summary>
    public void SetAttackIntervalByLevel()
    {
        switch (charaController.level)
        {
            case 1:
                attackInterval = 3.0f;
                break;

            case 2:
                attackInterval = 2.5f;
                break;

            case 3:
                attackInterval = 2.0f;
                break;

            case 4:
                attackInterval = 1.5f;
                break;

            case 5:
                attackInterval = 1.0f;
                break;

            case 6:
                attackInterval = 0.5f;
                break;
        }
    }
}
