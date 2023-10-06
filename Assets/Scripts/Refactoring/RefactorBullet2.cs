using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 追尾弾
/// </summary>
public class RefactorBullet2 : BulletBase
{
    private EnemyController target;  //追尾対象の敵

    [SerializeField] private float bulletSpeed;


    void Update()
    {
        if (!target)
        {
            return;
        }

        //追尾対象を追いかける
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, bulletSpeed * Time.deltaTime);
    }

    public override void Shoot(Vector2 direction) { }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            //追尾対象のリストから削除
            GameData.instance.targetList.Remove(target);

            //プールに戻す
            ReleaseBullet();
        }
    }
}
