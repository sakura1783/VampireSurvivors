using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 雷
/// </summary>
public class RefactorBullet4 : BulletBase
{
    private float destroyTime = 0.3f;
    public float DestroyTime => destroyTime;


    public override void Shoot(Vector2 direction) { }
}
