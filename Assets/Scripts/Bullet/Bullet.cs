using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// デフォルト
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;

    [SerializeField] private float destroyTime;


    /// <summary>
    /// バレット発射
    /// </summary>
    public void Shoot(Vector2 direction)
    {
        if (TryGetComponent(out Rigidbody2D rb))
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
