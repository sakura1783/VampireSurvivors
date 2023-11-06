using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// デフォルト弾
/// </summary>
public class RefactorBullet0 : BulletBase
{
    [SerializeField] private float bulletSpeed;

    [SerializeField] private float destroyTime = 5;


    /// <summary>
    /// 弾発射
    /// </summary>
    /// <param name="direction"></param>
    public override void Shoot(Vector2 direction)
    {
        SetUpBullet(hoge);

        if (rb)
        {
            rb.AddForce(direction * bulletSpeed);
        }

        //プールに戻す
        ReleaseBullet(destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            //プールに戻す
            ReleaseBullet();
        }
    }
}
