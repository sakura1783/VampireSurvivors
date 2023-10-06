using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefactorBullet0 : BulletBase
{
    [SerializeField] private float bulletSpeed;


    /// <summary>
    /// 弾発射
    /// </summary>
    /// <param name="direction"></param>
    public override void Shoot(Vector2 direction)
    {
        SetUpBullet();

        if (rb)
        {
            rb.AddForce(direction * bulletSpeed);
        }

        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
