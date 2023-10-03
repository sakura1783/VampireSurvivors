using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefactorBullet0 : BulletBase
{
    /// <summary>
    /// 弾発射
    /// </summary>
    /// <param name="direction"></param>
    public override void Shoot(Vector2 direction)
    {
        //SetUpBullet();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
