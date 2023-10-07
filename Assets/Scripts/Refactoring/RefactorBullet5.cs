using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 凍結魔法
/// </summary>
public class RefactorBullet5 : BulletBase
{
    [SerializeField] private float bulletSpeed;

    [SerializeField] private float destroyTime;  //短いと処理途中で行われない処理が出てきてしまうのでこのバレットのみ長めに設定

    [SerializeField] private GameObject iceEffectPrefab;

    [SerializeField] private float frozenTime;
    public float FrozenTime => frozenTime;

    private EnemyController frozenEnemy;  //凍っている(凍らせた)エネミー

    private Transform temporaryObjectsPlace;


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parentTran"></param>
    public override void SetUpBullet<T>(T parentTran)
    {
        if (parentTran is Transform temporaryObjectsPlace)
        {
            this.temporaryObjectsPlace = temporaryObjectsPlace;
        }
    }

    public override void Shoot(Vector2 direction)
    {
        SetUpBullet();

        if (rb)
        {
            rb.AddForce(direction * bulletSpeed);
        }

        //プールに戻す
        ReleaseBullet(destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out frozenEnemy))
        {
            if (!frozenEnemy.isFrozen)
            {
                frozenEnemy.isFrozen = true;

                //敵の上に氷塊の画像を生成
                StartCoroutine(GenerateIceEffect(col));
            }

            //ReleaseBullet()でSetActiveをfalseにしてしまうと思い通りの処理にならないのでここでは弾のコライダーと描画処理を無くすだけ
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private IEnumerator GenerateIceEffect(Collider2D col)
    {
        GameObject effect = Instantiate(iceEffectPrefab, col.transform.position, Quaternion.identity);

        effect.transform.SetParent(temporaryObjectsPlace);

        yield return new WaitForSeconds(frozenTime);

        //氷が解けたらエフェクトを破壊
        Destroy(effect);

        frozenEnemy.isFrozen = false;
    }
}
