using UnityEngine;
using UnityEngine.Pool;

public abstract class BulletGeneratorBase : MonoBehaviour, IGeneratable
{
    protected int bulletLevel = 1;
    public int BulletLevel => bulletLevel;

    //[SerializeField] protected float bulletSpeed;  //BulletDataがない時点のデバッグ用として利用できる

    //[SerializeField] protected BulletBase bulletPrefab;  //同上

    protected CharaController charaController;

    protected float bulletTimer;

    protected Transform temporaryObjectsPlace;

    protected BulletDataSO.BulletData bulletData;
    public BulletDataSO.BulletData BulletData => bulletData;

    protected bool isSetUp = false;

    protected int hoge;  //GenerateBulletメソッドは1つに統一したいのでT型に代入する適当な変数hogeを作る

    protected IObjectPool<BulletBase> bulletPool;  //弾用のオブジェクトプール(参照はインターフェースで持っておくとスタック型(スタック型のオブジェクトプールは、新しいオブジェクトの取得と返却が高速だが、オブジェクトの返却順序は保証されない)と連結リスト型(連結リスト型のオブジェクトプールはオブジェクトの取得と返却が一定の時間で行われるため、一定の性能が期待できるが、一部の操作がスタック型より遅い可能性がある)の実装を変更可能。newのタイミングで実装変更できる)

    [SerializeField] protected int initialPoolSize = 5;  //オブジェクトプールの初期サイズ(最初に作っておく弾の数)


    protected void Awake()
    {
        //オブジェクトプールの初期設定。newだが、インスタンスを作るわけではない
        bulletPool = new ObjectPool<BulletBase>
        (
            createFunc: () => Create(),
            actionOnGet: OnGetFromPool,
            actionOnRelease: target => target.gameObject.SetActive(false),
            actionOnDestroy: target => Destroy(target.gameObject),
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 1000
        );

        Debug.Log(bulletPool.CountInactive); //この時点ではインスタンスなしなので、0が出る

        //デバッグ用
        //BulletBase bullet = bulletPool.Get();  //ないので作る
        //bulletPool.Release(bullet);  //非表示にする
        //BulletBase bullet2 = bulletPool.Get();  //再表示する
        //bulletPool.Get();  //ないので作る

        Debug.Log("弾のオブジェクトプール 初期化完了");
    }

    protected virtual void Update()
    {
        if (!isSetUp)
        {
            return;
        }

        //ポップアップ表示中は生成しない
        if (charaController.GameManager.IsProcessingPaused)
        {
            return;
        }

        //アタックポーションの効果中は攻撃速度を1.5倍にする
        bulletTimer += charaController.Item.IsAttackTimeReduced ? Time.deltaTime * (float)1.5f : Time.deltaTime;

        if (bulletTimer >= bulletData.attackInterval)
        {
            bulletTimer = 0;

            GenerateBullet(charaController.Direction, hoge);
        }
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="charaController"></param>
    public virtual void SetUpBulletGenerator(CharaController charaController, BulletDataSO.BulletData bulletData, Transform place = null)
    {
        this.charaController = charaController;

        temporaryObjectsPlace = charaController.temporaryObjectsPlace;  //4つのクラスで利用しているので共通化する。使わないクラスでは無視させる

        this.bulletData = bulletData;

        isSetUp = true;

        Debug.Log("BulletGeneratorBase 初期設定完了");
    }

    /// <summary>
    /// bulletPool.Get()メソッドにより、createFunkとして実行される
    /// </summary>
    /// <returns></returns>
    private BulletBase Create()
    {
        BulletBase bulletInstance = Instantiate(bulletData.bulletPrefab);

        //参照を与えておく(依存性注入する)ことによって、Bullet側でReleaseできる
        bulletInstance.ObjectPool = bulletPool;

        Debug.Log("新しく弾を生成");
        Debug.Log(bulletInstance.ObjectPool);

        return bulletInstance;
    }

    /// <summary>
    /// bulletPool.Get()メソッドにより、actionOnGetとして実行される
    /// </summary>
    /// <param name="target"></param>
    void OnGetFromPool(BulletBase target)
    {
        target.gameObject.SetActive(true);

        Debug.Log("プールから弾を取得");
    }

    /// <summary>
    /// 外部クラスより実行する
    /// bulletPool.Get()メソッドにより、オブジェクトプールから弾を取り出して戻す→ OnGetFromPoolメソッドが実行される
    /// プール内に弾がない場合には新しく生成して戻す→ Createメソッドが実行される
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public BulletBase GetBullet(Vector3 pos, Quaternion rotation)
    {
        BulletBase bullet = bulletPool.Get();

        bullet.transform.position = pos;
        bullet.transform.rotation = rotation;

        Debug.Log("生成命令(弾)");

        return bullet;
    }

    //public abstract void GenerateBullet();

    //public abstract void GenerateBullet(Vector2 direction);

    public abstract void GenerateBullet<T>(Vector2 direction, T t);

    /// <summary>
    /// 弾が最大レベルに達しているか判定
    /// </summary>
    /// <returns></returns>
    public bool ReachedMaxLevel()
    {
        return bulletLevel >= bulletData.maxLevel;
    }

    /// <summary>
    /// バレットのレベルアップ
    /// </summary>
    public virtual void LevelUpBullet()
    {
        //最大値を超えない範囲で加算(前置きインクリメント(++変数)の書式で書くことで、計算結果を反映して処理を進めることができる)
        bulletLevel = Mathf.Min(bulletData.maxLevel, ++bulletLevel);  //Mathf.Min()で2つ以上の値から最小値を返す
    }
}
