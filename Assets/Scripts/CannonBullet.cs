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
            //無敵でない場合のみ
            if (!charaController.Item.IsInvincible)
            {
                if (charaController.GameManager.IsGameUp)
                {
                    return;
                }

                //エフェクト生成
                GameObject effect = Instantiate(EffectManager.instance.GetEffect(EffectName.Hit), transform.position, Quaternion.identity);

                effect.transform.SetParent(charaController.TreasureChestGenerator.TemporaryObjectsPlace);

                //SE再生
                AudioManager.instance.PlaySE(SeType.Hit);

                //プレイヤーのHPを減らす
                //charaController.UpdateHp(-attackPoint);
                //シールド中なら、ダメージを1減らす
                charaController.UpdateHp(-(attackPoint += charaController.Item.IsShielded ? -1 : 0));
            }

            Destroy(gameObject);
        }
    }
}
