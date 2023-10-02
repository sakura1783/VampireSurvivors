using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyGeneratorObjectPool : MonoBehaviour
{
    [SerializeField] private float generateInterval;
    public float GenerateInterval
    {
        get => generateInterval;
        set => generateInterval = value;
    }

    [SerializeField] private EnemyController enemyPrefab;

    [SerializeField] private GameManager gameManger;

    [SerializeField] private int initialPoolSize = 5;

    private IObjectPool<EnemyController> enemyPool;


    //コンストラクタ(インスタンスを初期化する)メソッド
    void Awake()
    {
        //オブジェクトプールの初期設定
        enemyPool = new ObjectPool<EnemyController>  //ObjectPoolはジェネリック<T>なので利用する時に型を指定する。
        (  //第1〜第4引数は特定のイベント発生時の処理、第5〜第7引数はオブジェクトプールの設定値
            createFunc: () => Create(),  //IObjectPool.Get()の結果、オブジェクトプール内にオブジェクトがない場合にこの処理が動く。
            actionOnGet: OnGetFromPool,  //メソッド作成して登録できる  //IObjectPool.Get()の結果、オブジェクトプール内にオブジェクトがある場合はこの処理が動く。
            actionOnRelease: target => target.gameObject.SetActive(false),  //IObjectPool.Release()により動く。
            actionOnDestroy: target => Destroy(target.gameObject),  //第7引数のmaxSizeに達した際(オブジェクトをプールに戻せなかった時)に自動的に動く。
            collectionCheck: true,  //trueで、オブジェクトのインスタンスがプールに戻される時に自動的に実行される。同一のインスタンスが登録されているか調べ、すでに登録がある場合は例外がスローされる。
            defaultCapacity: 10,  //最初にオブジェクトプール内にオブジェクトを生成する命令が来た時、この値の分だけ、オブジェクトプール内にオブジェクトのインスタンスを生成する。
            maxSize: 1000  //オブジェクトプール内に保持できるオブジェクトの総数
        );

        //デバッグ用
        //EnemyController enemy = enemyPool.Get();  //ないので作る
        //enemyPool.Release(enemy);  //非表示にする
        //EnemyController enemy2 = enemyPool.Get();  //再表示する
        //enemyPool.Get();  //ないので作る

        Debug.Log("オブジェクトプール 初期化完了");

        //StartCoroutine(GenerateEnemy(chara));
    }

    /// <summary>
    /// bulletPool.Get()メソッドにより、createFunkとして実行される
    /// </summary>
    /// <returns></returns>
    private EnemyController Create()
    {
        EnemyController enemyInstance = Instantiate(enemyPrefab);

        //参照を与えておく(依存性注入する)ことで、Bullet側でReleaseできる
        enemyInstance.ObjectPool = enemyPool;

        GameData.instance.enemiesList.Add(enemyInstance);

        Debug.Log("生成");
        Debug.Log(enemyInstance.ObjectPool);

        return enemyInstance;
    }

    /// <summary>
    /// enemyPool.Get()メソッドにより、actionOnGetとして実行される
    /// </summary>
    /// <param name="target"></param>
    private void OnGetFromPool(EnemyController target)
    {
        target.gameObject.SetActive(true);
    }

    /// <summary>
    /// enemyPool.Get()メソッドにより、オブジェクトプールから敵を取り出して戻す→ OnGetFromPoolメソッドが実行される
    /// プール内に敵がいない場合には新しく生成して戻す→ Createメソッドが実行される
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public EnemyController GetEnemy(Vector3 position, Quaternion rotation)
    {
        //自動分岐
        EnemyController enemy = enemyPool.Get();
        enemy.transform.position = position;
        enemy.transform.rotation = rotation;

        Debug.Log("取得");
        return enemy;
    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    /// <param name="charaController"></param>
    /// <returns></returns>
    public IEnumerator GenerateEnemy(CharaController charaController)
    {
        while (!gameManger.IsGameUp)
        {
            //TODO 敵を生成しない場合の条件を追加する
            if (gameManger.IsDisplayPopUp || gameManger.IsDisplayTitlePopUp || gameManger.IsDisplayResultPopUp)
            {
                yield return null;

                continue;
            }

            yield return new WaitForSeconds(generateInterval);

            //通常の生成方法。今回は利用しない
            //EnemyController enemy = Instantiate(enemyPrefab, transform);

            //オブジェクトプールを利用して敵を生成/表示
            EnemyController enemy = GetEnemy(transform.position, transform.rotation);

            enemy.SetUpEnemyController(charaController);
        }
    }
}
