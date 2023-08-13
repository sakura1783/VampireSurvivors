using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet3 : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;

    [SerializeField] private float destroyTime;


    /// <summary>
    /// バレット発射
    /// </summary>
    /// <param name="direction"></param>
    public void Shoot(Vector2 direction)
    {
        if (TryGetComponent(out Rigidbody2D rb))
        {
            rb.AddForce(direction * bulletSpeed);
        }

        Destroy(gameObject, destroyTime);
    }
}
