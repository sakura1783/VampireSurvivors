using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザー
/// </summary>
public class RefactorBullet3 : BulletBase
{
    [SerializeField] private float bulletSpeed;

    [SerializeField] private float destroyTime = 5;


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
}
