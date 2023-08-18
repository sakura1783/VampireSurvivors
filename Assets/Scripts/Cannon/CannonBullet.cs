using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;

    [SerializeField] private float destroyTime;

    [SerializeField] private int attackPoint;

    private CharaController charaController;


    /// <summary>
    /// バレット発射
    /// </summary>
    /// <param name="direction"></param>
    public void Shoot(CharaController charaController, Vector2 direction)
    {
        this.charaController = charaController;

        if (TryGetComponent(out Rigidbody2D rb))
        {
            rb.AddForce(direction * bulletSpeed);
        }

        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            //プレイヤーのHpを減らす
            charaController.UpdateHp(-attackPoint);

            Destroy(gameObject);
        }
    }
}
