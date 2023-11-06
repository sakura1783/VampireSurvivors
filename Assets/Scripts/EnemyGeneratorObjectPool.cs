using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System.Linq;

public class EnemyGeneratorObjectPool : MonoBehaviour
{
    //[SerializeField] private float generateInterval;
    //public float GenerateInterval
    //{
    //    get => generateInterval;
    //    set => generateInterval = value;
    //}

    //[SerializeField] private EnemyController enemyPrefab;

    private GameManager gameManager;

    protected GenerateData generateData;

    protected float currentGenerateInterval;

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

        Debug.Log("敵のオブジェクトプール 初期化完了");

        //StartCoroutine(GenerateEnemy(chara));
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="generateData"></param>
    /// <param name="gameManager"></param>
    /// <param name="charaController"></param>
    public void SetUpEnemyGenerator(GenerateData generateData, GameManager gameManager, CharaController charaController)
    {
        this.generateData = generateData;
        this.gameManager = gameManager;

        //敵生成の初期インターバル値の設定
        currentGenerateInterval = generateData.generateInterval;

        //生成開始
        StartCoroutine(GenerateEnemy(charaController));
    }

    /// <summary>
    /// bulletPool.Get()メソッドにより、createFunkとして実行される
    /// </summary>
    /// <returns></returns>
    private EnemyController Create()
    {
        EnemyController enemyInstance = Instantiate(generateData.enemyPrefab);

        //参照を与えておく(依存性注入する)ことで、Bullet側でReleaseできる
        enemyInstance.ObjectPool = enemyPool;

        //GameData.instance.enemiesList.Add(enemyInstance);

        Debug.Log("新しく敵を生成");
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

        Debug.Log("プールから敵を取得");
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
        //自動分岐。ここの分岐で第1または第2引数(30/31行目)の命令が動く
        EnemyController enemy = enemyPool.Get();
        enemy.transform.position = position;
        enemy.transform.rotation = rotation;

        GameData.instance.enemiesList.Add(enemy);

        Debug.Log("生成命令(敵)");
        return enemy;
    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    /// <param name="charaController"></param>
    /// <returns></returns>
    public IEnumerator GenerateEnemy(CharaController charaController)
    {
        while (!gameManager.IsGameUp)
        {
            if (gameManager.IsProcessingPaused)
            {
                yield return null;

                continue;
            }

            yield return new WaitForSeconds(currentGenerateInterval);

            //通常の生成方法。今回は利用しない
            //EnemyController enemy = Instantiate(enemyPrefab, transform);

            //オブジェクトプールを利用して敵を生成/表示
            EnemyController enemy = GetEnemy(transform.position, transform.rotation);

            enemy.SetUpEnemyController(charaController);
        }
    }

    /// <summary>
    /// レベルアップ時のインターバル値の補正チェック。該当するレベルの場合にはインターバル値を補正
    /// </summary>
    /// <param name="newCharaLevel"></param>
    public void CheckGenerateInterval(int newCharaLevel)
    {
        for (int i = 0; i < generateData.upgradeDatas.Length; i++)
        {
            if (generateData.upgradeDatas[i].level == newCharaLevel)
            {
                currentGenerateInterval -= generateData.upgradeDatas[i].offsetInterval;

                break;
            }
        }

        //foreachの場合
        //foreach (GenerateData.UpgradeData upgradeData in generateData.upgradeDatas)
        //{
        //    if (upgradeData.level == newCharaLevel)
        //    {
        //        currentGenerateInterval -= upgradeData.offsetInterval;

        //        break;
        //    }
        //}

        //上記をLINQで記述
        //GenerateData.UpgradeData matchingUpgrade = generateData.upgradeDatas.FirstOrDefault(data => data.level == newCharaLevel);

        //nullの可能性がある(該当するレベルのデータがない時)ので、安全のためnullチェックをしてから計算する
        //if (matchingUpgrade != null)
        //{
        //    currentGenerateInterval -= matchingUpgrade.offsetInterval;
        //}
    }
}
