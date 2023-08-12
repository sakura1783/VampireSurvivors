using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet2 : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;

    [SerializeField] private float destroyTime;

    private EnemyController target;  //追尾対象の敵


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

    /// <summary>
    /// 一番近い敵を見つける(それぞれ違う敵を追尾対象とする)
    /// </summary>
    /// <returns></returns>
    public void FindNearestEnemy(Vector2 direction)
    {
        float nearestDistance = float.MaxValue;

        foreach (EnemyController enemy in GameData.instance.enemiesList)
        {
            //enemyがtargetListの中にすでに含まれている(すでにどれかの追尾弾の追尾対象となっている)場合、それは追尾対象としない
            for (int i = 0; i < GameData.instance.targetList.Count; i++)
            {
                if (enemy == GameData.instance.targetList[i])
                {
                    continue;
                }
            }

            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (nearestDistance > distance)
            {
                nearestDistance = distance;

                target = enemy;
            }
        }

        //最終的なターゲットをリストに追加(追尾弾それぞれのターゲットが被らないようにする)
        GameData.instance.targetList.Add(target);

        //バレット発射
        //ShootChaseBullet(direction);

        Destroy(gameObject, destroyTime);
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
