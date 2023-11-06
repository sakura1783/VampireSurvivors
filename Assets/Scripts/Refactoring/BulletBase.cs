using UnityEngine;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;

//抽象クラス(abstractクラス)。抽象メソッド(宣言部分のみが記述され、処理の本体が存在しない)を1つ以上持っているクラス。
public abstract class BulletBase : MonoBehaviour, IShootable
{
    protected Rigidbody2D rb;
    public Rigidbody2D Rb => rb;

    protected int hoge;

    private IObjectPool<BulletBase> objectPool;
    public IObjectPool<BulletBase> ObjectPool  //弾にObjectPoolへの参照を与えるプロパティ
    {
        get => objectPool;
        set => objectPool = value;
    }

    public bool isReleasedToPool = false;  //弾がすでにプールに戻されているか


    /// <summary>
    /// 弾をオブジェクトプールに戻す
    /// </summary>
    public virtual async void ReleaseBullet(float destroyTime = 0)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(destroyTime));

        if (!isReleasedToPool)
        {
            isReleasedToPool = true;

            objectPool.Release(this);

            Debug.Log(objectPool);
        }
    }

    /// <summary>
    /// 初期設定用の仮想メソッド(動作内容を変更できるメソッド。virtual/override)
    /// このメソッドをオーバーライドして、特定の型に基づいた初期設定を行うことができる。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t">初期設定に使用するジェネリック型のパラメータ。メソッドの実行側で適切な型を指定する。</param>
    public virtual void SetUpBullet<T>(T t)
    {
        isReleasedToPool = false;

        TryGetComponent(out rb);
    }

    /// <summary>
    /// 上記メソッドのオーバーロード
    /// </summary>
    //public virtual void SetUpBullet()
    //{
    //    TryGetComponent(out rb);
    //}

    /// <summary>
    /// 弾を発射する
    /// 抽象メソッドとしてIShootableインターフェースを実装している。抽象メソッドであるため、このメソッドを子クラスでオーバーライドして、具体的な弾の発射挙動を実装する。
    /// </summary>
    /// <param name="direction"></param>
    public abstract void Shoot(Vector2 direction);
}
