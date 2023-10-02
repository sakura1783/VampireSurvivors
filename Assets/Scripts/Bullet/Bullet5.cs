using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 凍結魔法
/// </summary>
public class Bullet5 : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;

    [SerializeField] private float destroyTime;  //短いと処理途中で削除されて行われない処理が出てきてしまうのでこのバレットのみ長めに設定

    [SerializeField] private GameObject iceEffectPrefab;  //凍っている敵の上に被せる氷塊の画像

    public float frozenTime;  //凍る時間

    private EnemyController frozenEnemy;  //凍っている(凍らせた)エネミー

    private Transform temporaryObjectsPlace;


    /// <summary>
    /// バレット発射
    /// </summary>
    public void Shoot(Vector2 direction, Transform temporaryObjectsPlace)
    {
        this.temporaryObjectsPlace = temporaryObjectsPlace;

        if (TryGetComponent(out Rigidbody2D rb))
        {
            rb.AddForce(direction * bulletSpeed);
        }

        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //if (col.CompareTag("Enemy"))
        //{
        if (col.TryGetComponent(out frozenEnemy))
        {
            if (!frozenEnemy.isFrozen)
            {
                //敵の上に氷塊の画像を生成
                StartCoroutine(GenerateIceEffect(col));

                frozenEnemy.isFrozen = true;
            }

            //Destroy(gameObject);  //ここでDestroyしてしまうと処理が途中で終了してしまい思い通りの動きにならない

            //Destroyではなくここではコライダーや描画処理などを無くすだけ(※SetActiveをtrueにするとコルーチンが動かなくなるらしいので注意)
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }
        //}
    }

    /// <summary>
    /// 敵の上に氷塊の画像を生成
    /// </summary>
    private IEnumerator GenerateIceEffect(Collider2D col)
    {
        //Debug.Log("氷を生成します");

        GameObject effect = Instantiate(iceEffectPrefab, col.transform.position, Quaternion.identity);

        effect.transform.SetParent(temporaryObjectsPlace);

        yield return new WaitForSeconds(frozenTime);

        //氷が解けたらエフェクトを破壊
        Destroy(effect);

        frozenEnemy.isFrozen = false;

        //Debug.Log("氷が溶けました");
    }
}
