using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyController : MonoBehaviour
{
    public int attackPoint;

    public int exp;

    public bool isFrozen = false;  //凍っているかどうか。trueの場合は一定時間動きを止める

    private Animator enemyAnim;

    private CharaController charaController;

    private TreasureChestGenerator treasureChestGenerator;

    private GameManager gameManager;

    private IObjectPool<EnemyController> objectPool;
    public IObjectPool<EnemyController> ObjectPool
    {
        get => objectPool;
        set => objectPool = value;
    }

    private bool isReleasedToPool = false;

    [SerializeField] private NavMeshAgent2D navMeshAgent2D;

    [SerializeField] private int itemGenerateRate;

    [SerializeField] private int score;


    void Update()
    {
        //ポップアップ表示中は動きを止める
        if (charaController.GameManager.IsTimePaused)
        {
            return;
        }

        if (isFrozen)
        {
            return;
        }

        navMeshAgent2D.destination = charaController.transform.position;

        ChangeAnimDirection();
    }

    /// <summary>
    /// 敵をオブジェクトプールに戻す
    /// </summary>
    public void ReleaseEnemy()
    {
        if (!isReleasedToPool)
        {
            isReleasedToPool = true;

            objectPool.Release(this);

            Debug.Log("ObjectPool");
        }
    }

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="charaController"></param>
    public void SetUpEnemyController(CharaController charaController)
    {
        this.charaController = charaController;
        this.treasureChestGenerator = charaController.TreasureChestGenerator;
        this.gameManager = charaController.GameManager;

        transform.GetChild(0).TryGetComponent(out enemyAnim);
    }

    /// <summary>
    /// アニメ変更
    /// </summary>
    private void ChangeAnimDirection()
    {
        Vector2 direction = (charaController.transform.position - transform.position).normalized;

        enemyAnim.SetFloat("X", direction.x);
        enemyAnim.SetFloat("Y", direction.y);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet"))
        {
            if (gameManager.IsGameUp)
            {
                return;
            }

            //エフェクト生成
            //GameObject effectPrefab = EffectManager.instance.GetEffect(EffectName.enemyDown);
            //GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            GameObject effect = Instantiate(EffectManager.instance.GetEffect(EffectName.Hit), transform.position, Quaternion.identity);

            effect.transform.SetParent(treasureChestGenerator.TemporaryObjectsPlace);

            //Destroy(effect, 1.5f);

            //SE再生
            AudioManager.instance.PlaySE(SeType.Hit);

            //リストから削除
            GameData.instance.enemiesList.Remove(this);

            //倒した敵の数を加算
            gameManager.AddKillEnemyCount();

            //スコア加算
            gameManager.AddScore(score);

            //Destroy(gameObject);

            //オブジェクトプールに戻す
            ReleaseEnemy();

            //宝箱の生成
            int randomNo = Random.Range(0, 100);

            if (randomNo < itemGenerateRate)
            {
                treasureChestGenerator.GenerateTreasureChest(transform);
            }

            //Expの加算
            charaController.AddExp(exp);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (gameManager.IsGameUp)
            {
                return;
            }

            //無敵でない場合のみ
            if (!charaController.Item.IsInvincible)
            {
                //プレイヤーのHPを減らす
                //charaController.UpdateHp(-attackPoint);
                //シールド中なら、ダメージを1減らす
                charaController.UpdateHp(-(attackPoint += charaController.Item.IsShielded ? -1 : 0));
            }

            //エフェクト生成
            GameObject effect = Instantiate(EffectManager.instance.GetEffect(EffectName.Hit), transform.position, Quaternion.identity);

            effect.transform.SetParent(treasureChestGenerator.TemporaryObjectsPlace);

            //Destroy(effect, 1.5f);

            //SE再生
            AudioManager.instance.PlaySE(SeType.Hit);

            //リストから削除
            GameData.instance.enemiesList.Remove(this);

            //※プレイヤーと敵が直接ぶつかった場合は敵キル数、スコアともに加算しない

            //Destroy(gameObject);

            ReleaseEnemy();
        }
    }
}
