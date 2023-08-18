using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 追尾弾
/// </summary>
public class Bullet2 : MonoBehaviour
{
    public EnemyController target;  //追尾対象の敵

    [SerializeField] private float bulletSpeed;

    [SerializeField] private float destroyTime;


    void Update()
    {
        if (!target)
        {
            return;
        }

        //追尾対象を追いかける
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, bulletSpeed * Time.deltaTime);
    }

    /// <summary>
    /// バレット発射
    /// </summary>
    //private void ShootChaseBullet(Vector2 direction)
    //{
    //    //Vector2 direction = target.transform.position;

    //    if (TryGetComponent(out Rigidbody2D rb))
    //    {
    //        rb.AddForce(bulletSpeed * direction);
    //    }

    //    Destroy(gameObject, destroyTime);
    //}

    ///// <summary>
    ///// 一番近い敵を見つける(それぞれ違う敵を追尾対象とする)
    ///// </summary>
    ///// <returns></returns>
    //public void FindNearestEnemy(Vector2 direction)
    //{
    //    target = null;

    //    //float nearestDistance = float.MaxValue;

    //    //enemiesListの中身をOrderBy()で小さい順に並べ替える(距離順にソート)
    //    var sortedEnemies = GameData.instance.enemiesList.OrderBy(enemy => Vector2.Distance(transform.position, enemy.transform.position));

    //    //foreach (EnemyController enemy in GameData.instance.enemiesList)
    //    //{
    //    //    //enemyがtargetListの中にすでに含まれている(すでにどれかの追尾弾の追尾対象となっている)場合、それは追尾対象としない
    //    //    for (int i = 0; i < GameData.instance.targetList.Count; i++)
    //    //    {
    //    //        if (enemy == GameData.instance.targetList[i])
    //    //        {
    //    //            continue;  //ここでcontinueしても次のfor文の処理に行くだけで、下の処理はスキップされないので、同じ敵をtargetとして設定してしまう。つまり、「あ、List に入ってるね」とチェックだけして、そのまま、その敵を省かずに距離のチェックに入るので同じ敵を対象にしてしまう
    //    //        }
    //    //    }

    //    //    float distance = Vector2.Distance(transform.position, enemy.transform.position);

    //    //    if (nearestDistance > distance)
    //    //    {
    //    //        nearestDistance = distance;

    //    //        target = enemy;
    //    //    }
    //    //}

    //    foreach (EnemyController enemy in sortedEnemies)
    //    {
    //        //enemyがtargetListに含まれている場合はスキップ
    //        if (GameData.instance.targetList.Exists(existingTarget => existingTarget == enemy))
    //        {
    //            continue;
    //        }

    //        target = enemy;

    //        break;  //最も近い敵が見つかったらループを終了
    //    }

    //    if (target)
    //    {
    //        //最終的なターゲットをリストに追加(追尾弾それぞれのターゲットが被らないようにする)
    //        GameData.instance.targetList.Add(target);

    //        //バレット発射
    //        //ShootChaseBullet(direction);
    //    }

    //    Destroy(gameObject, destroyTime);
    //}

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
