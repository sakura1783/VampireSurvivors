using UnityEngine;
using UnityEngine.Pool;

public abstract class BulletGeneratorBase : MonoBehaviour, IGeneratable
{
    [SerializeField] protected float bulletSpeed;

    [SerializeField] protected BulletBase bulletPrefab;

    protected CharaController charaController;

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

    /// <summary>
    /// bulletPool.Get()メソッドにより、createFunkとして実行される
    /// </summary>
    /// <returns></returns>
    private BulletBase Create()
    {
        BulletBase bulletInstance = Instantiate(bulletPrefab);

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

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="charaController"></param>
    public virtual void SetUpBulletGenerator(CharaController charaController)
    {
        this.charaController = charaController;

        Debug.Log("初期設定 完了");
    }

    public abstract void GenerateBullet(Vector2 direction);

    public abstract void GenerateBullet<T>(Vector2 direction, T t);
}
