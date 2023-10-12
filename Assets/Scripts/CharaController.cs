using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharaController : MonoBehaviour
{
    public int charaLevel = 1;

    //public List<BulletDataSO.BulletData> bulletDatasList = new();  //ポップアップに渡す情報。レベルアップ時にポップアップを出すのでここに宣言する

    public LevelupPopUp levelupPop;

    //ここから各BulletGeneratorへのアサイン用変数
    //public Bullet1 bullet1Prefab;
    //public Bullet2 bullet2Prefab;
    //public Bullet3 bullet3Prefab;
    //public Bullet4 bullet4Prefab;
    //public Bullet5 bullet5Prefab;

    public Transform temporaryObjectsPlace;

    public int maxHp;

    public int hp;

    [SerializeField] private float moveSpeed;

    //[SerializeField] private float limitPosX;
    //[SerializeField] private float limitPosY;
    [SerializeField] private Transform leftBottomLimitTran;
    [SerializeField] private Transform rightTopLimitTran;

    //[SerializeField] private MapManager mapManager;

    //[SerializeField] private Collider2D leftStairTrigger;
    //[SerializeField] private Collider2D rightStairTrigger;  //isClimbedStairを判定するための階段のコライダー

    //[SerializeField] private float attackInterval;

    //[SerializeField] private Bullet bulletPrefab;

    //[SerializeField] private Transform temporaryObjectsPlace;

    [SerializeField] private int needExpForLevelUp;  //レベルアップに必要なExp

    [SerializeField] private int totalExp;  //現在保持しているExp

    //[SerializeField] private BulletGenerator bulletGenerator;
    //[SerializeField] private BulletGenerator1 bulletGenerator1;
    //[SerializeField] private BulletGenerator2 bulletGenerator2;
    //[SerializeField] private BulletGenerator3 bulletGenerator3;
    //[SerializeField] private BulletGenerator4 bulletGenerator4;
    //[SerializeField] private BulletGenerator5 bulletGenerator5;

    //[SerializeField] private EnemyGenerator enemyGenerator;
    //[SerializeField] private EnemyGenerator enemyGenerator1;
    //[SerializeField] private EnemyGenerator enemyGenerator2;
    //[SerializeField] private EnemyGeneratorObjectPool enemyGenerator;
    //[SerializeField] private EnemyGeneratorObjectPool enemyGenerator1;
    //[SerializeField] private EnemyGeneratorObjectPool enemyGenerator2;

    [SerializeField] private UIManager uiManager;

    [SerializeField] private GameManager gameManager;
    public GameManager GameManager => gameManager;

    [SerializeField] private Item item;
    public Item Item => item;

    [SerializeField] private TreasureChestGenerator treasureChestGenerator;  //EnemyControllerに渡す用
    public TreasureChestGenerator TreasureChestGenerator => treasureChestGenerator;

    [SerializeField] private ResultPopUp resultPop;

    private Animator charaAnim;
    public Animator CharaAnim => charaAnim;

    private float charaScale;  //キャラの左右アニメの設定で利用する

    private Vector2 direction;  //キャラが向いている方向
    public Vector2 Direction => direction;

    private int addPoint = 5;  //needExpForLevelUp変数に加算するポイント(レベルが上がるにつれて、必要なExpも増える)

    //各BulletGenerator
    //private BulletGenerator bulletGenerator;
    //public BulletGenerator BulletGenerator => bulletGenerator;

    //private BulletGenerator1 bulletGenerator1;
    //public BulletGenerator1 BulletGenerator1 => bulletGenerator1;

    //private BulletGenerator2 bulletGenerator2;
    //public BulletGenerator2 BulletGenerator2 => bulletGenerator2;

    //private BulletGenerator3 bulletGenerator3;
    //public BulletGenerator3 BulletGenerator3 => bulletGenerator3;

    //private BulletGenerator4 bulletGenerator4; 
    //public BulletGenerator4 BulletGenerator4 => bulletGenerator4;

    //private BulletGenerator5 bulletGenerator5;
    //public BulletGenerator5 BulletGenerator5 => bulletGenerator5;

    //各バレットの攻撃のインターバル時間
    //private float defaultTimer;
    //private float bulletTimer0;
    //private float bulletTimer1;
    //private float bulletTimer2;
    //private float bulletTimer3;
    //private float bulletTimer4;
    //private float bulletTimer5;

    public List<BulletGeneratorBase> AttatchedBulletGeneratorList = new();

    private List<EnemyGeneratorObjectPool> enemyGeneratorList = new();

    [SerializeField] private Transform enemyGeneratorSetTran;  //EnemyGeneratorsをアサイン

    //[SerializeField] private EnemyGeneratorObjectPool enemyGeneratorPrefab;

    [SerializeField] private int defaultBulletNo = 0;


    //テスト用。終わったら消す
    //void Start()
    //{
    //    //direction = new Vector2(0, -1);  //プレイヤーの初期方向をセット。何も設定をしないと最初directionは(0, 0)なので、これでプレイヤーの向きと同期する
    //    //bulletGenerator1.SetUpBulletGenerator1();
    //    //bulletGenerator1.PrepareGenerateBullet(direction);

    //    //各BulletGeneratorがアタッチされているか確認  //メソッドにする。最初だけでなくレベルアップ時にも確認する必要があるため
    //    //TryGetComponent(out bulletGenerator4);  //取得できた場合はtrueを返し、取得できなかった場合はfalseを返す。取得できなかった場合も取得できないだけで、エラーは出ない

    //    //bulletGenerator4.SetAttackIntervalByLevel();
    //}

    void Update()
    {
        //ポップアップ表示中は動かない
        if (gameManager.IsProcessingPaused)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 tapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Move(tapPos);

            //現在のアニメを取得。0はアニメのレイヤー番号
            //animatorClipInfo = charaAnim.GetCurrentAnimatorClipInfo(0);
        }
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpCharaController()
    {
        hp = maxHp;

        uiManager.UpdateHpGauge();

        //プレイヤーの初期方向をセット
        direction = new Vector2(0, -1);  

        transform.GetChild(0).gameObject.TryGetComponent(out charaAnim);

        charaScale = transform.GetChild(0).localScale.x;  //キャラの向き変更に使う

        //StartCoroutine(PrepareAttack());

        //バレットのデータをリスト化
        //CreateBulletDatasList();

        //アタッチされているBulletGeneratorたちを各変数に入れる
        //AssainBulletGenerators();

        //LevelupPopUpの設定
        levelupPop.SetUpLevelupPopUp(this);  //bulletDatasList

        //初期武器の追加
        AttatchBulletGenerator(DataBaseManager.instance.GetBulletData(defaultBulletNo));

        //敵のジェネレーターを生成(CharaLevel1のものが該当してEnemyGeneratorが1つ生成される)
        CheckCreateEnemyGenerator();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move(Vector2 tapPos)
    {
        Vector2 newPos = Vector2.MoveTowards(transform.position, tapPos, moveSpeed * Time.deltaTime);

        //マップの範囲外にでないように制限をかける
        //newPos.x = Mathf.Clamp(newPos.x, -limitPosX, limitPosX);
        //newPos.y = Mathf.Clamp(newPos.y, -limitPosY, limitPosY);
        newPos.x = Mathf.Clamp(newPos.x, leftBottomLimitTran.position.x, rightTopLimitTran.position.x);
        newPos.y = Mathf.Clamp(newPos.y, leftBottomLimitTran.position.y, rightTopLimitTran.position.y);

        //transform.position = newPos;  //この時点でtransform.positionの値はnewPosになっている。よってここに書くと思い通りの挙動にならないのでコメントアウト

        //アニメーション
        direction = (newPos - (Vector2)transform.position).normalized;  //上ですでにtransform.positionの値はnewPosになっているのでDebugを入れて確認できる通り、directionの値は全て0になる。よって思い通りの挙動にならない。

        transform.position = newPos;  //記述する位置を修正。この処理はnewPosを計算などで使わなくなってから書くようにする。そうしないと、意図しない挙動になってしまう(今回だと、アニメーションが同期されない)

        charaAnim.SetFloat("X", direction.x);
        charaAnim.SetFloat("Y", direction.y);

        //左右アニメの切り替え(Scaleを変化させて左右移動にアニメを対応させる)
        Vector2 temp = transform.GetChild(0).localScale;  //<= temp(一時的な)変数に現在のlocalScaleの値を代入

        //temp.x = direction.x;  //目的地の方向をtemp変数に代入
        //if (temp.x < 0)
        //{
        //    //0よりも小さければScaleを1にする
        //    temp.x = charaScale;
        //}
        //else
        //{
        //    //0よりも大きければScaleを-1にする
        //    temp.x = -charaScale;
        //}

        //上の処理を三項演算子で記述
        temp.x = direction.x < 0 ? charaScale : -charaScale;

        transform.GetChild(0).localScale = temp;
    }

    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    ////階段のコライダーと接触した際、isClimbedStairの値を変え、コライダーのアクティブ状態を切り替える
    //    //if (col == leftStairTrigger || col == rightStairTrigger)
    //    //{
    //    //    if (!mapManager.isClimbedStairs)
    //    //    {
    //    //        mapManager.isClimbedStairs = true;

    //    //        mapManager.JudgeClimbedStairs();

    //    //        return;
    //    //    }

    //    //    if (mapManager.isClimbedStairs)
    //    //    {
    //    //        mapManager.isClimbedStairs = false;

    //    //        mapManager.JudgeClimbedStairs();

    //    //        return;
    //    //    }
    //    //}

    //    if (col.CompareTag("Enemy"))
    //    {
    //        Destroy(col.gameObject);

    //        UpdateHp(-col.GetComponent<EnemyController>().attackPoint);
    //    }
    //}

    /// <summary>
    /// 攻撃準備。バレットごとにインターバル時間を変える
    /// </summary>
    //private IEnumerator PrepareAttack()
    //{
    //    while (true)
    //    {
    //        //ポップアップ表示中は攻撃しない
    //        if (gameManager.IsDisplayPopUp || gameManager.IsDisplayTitlePopUp || gameManager.IsDisplayResultPopUp)
    //        {
    //            yield return null;

    //            continue;
    //        }

    //        //if (bulletGenerator4)
    //        //{
    //        //    bullet4Timer += Time.deltaTime;

    //        //    if (bullet4Timer >= bulletGenerator4.attackInterval)
    //        //    {
    //        //        bullet4Timer = 0;

    //        //        //Attack();
    //        //        bulletGenerator4.PrepareGenerateBullet();
    //        //    }
    //        //}
    //        //else
    //        //{
    //        //bulletTimer0 += Time.deltaTime;

    //        //if (bulletTimer0 >= attackInterval)
    //        //{
    //        //    bulletTimer0 = 0;

    //        //    Attack();
    //        //}
    //        //}

    //        //yield return new WaitForSeconds(attackInterval);

    //        //Attack();

    //        if (bulletGenerator)
    //        {
    //            //bulletTimer0 += Time.deltaTime;
    //            //アタックポーションの効果中は攻撃速度を1.5倍にする
    //            bulletTimer0 += item.IsAttackTimeReduced ? Time.deltaTime * (float)1.5f : Time.deltaTime;

    //            if (bulletTimer0 >= bulletDatasList[0].attackInterval)
    //            {
    //                bulletTimer0 = 0;

    //                bulletGenerator.PrepareGenerateBullet(direction);

    //                //Debug.Log("デフォルト弾発射");
    //            }
    //        }
    //        //if (bulletGenerator1)
    //        //{
    //        //    bulletTimer1 += Time.deltaTime;

    //        //    if (bulletTimer1 >= bulletDatasList[1].attackInterval)
    //        //    {
    //        //        bulletTimer1 = 0;

    //        //        bulletGenerator1.PrepareGenerateBullet(direction);

    //        //        Debug.Log("手裏剣発射");
    //        //    }
    //        //}
    //        if (bulletGenerator2)
    //        {
    //            //bulletTimer2 += Time.deltaTime;
    //            bulletTimer2 += item.IsAttackTimeReduced ? Time.deltaTime * (float)1.5f : Time.deltaTime;

    //            if (bulletTimer2 >= bulletDatasList[2].attackInterval)
    //            {
    //                bulletTimer2 = 0;

    //                StartCoroutine(bullet2Generator.GenerateBullet());

    //                //Debug.Log("追尾弾発射");
    //            }
    //        }
    //        if (bulletGenerator3)
    //        {
    //            //bulletTimer3 += Time.deltaTime;
    //            bulletTimer3 += item.IsAttackTimeReduced ? Time.deltaTime * (float)1.5f : Time.deltaTime;

    //            if (bulletTimer3 >= bulletDatasList[3].attackInterval)
    //            {
    //                bulletTimer3 = 0;

    //                bulletGenerator3.PrepareGenerateBullet(direction);

    //                //Debug.Log("レーザー発射");
    //            }
    //        }
    //        if (bulletGenerator4)
    //        {
    //            //bulletTimer4 += Time.deltaTime;
    //            bulletTimer4 += item.IsAttackTimeReduced ? Time.deltaTime * (float)1.5f : Time.deltaTime;

    //            if (bulletTimer4 >= bulletGenerator4.attackInterval)  //bullet4(雷)だけはattackIntervalが変化するのでbulletGenerator4から値をもらう
    //            {
    //                bulletTimer4 = 0;

    //                bulletGenerator4.PrepareGenerateBullet();

    //                //Debug.Log("雷発射");
    //            }
    //        }
    //        if (bulletGenerator5)
    //        {
    //            //bulletTimer5 += Time.deltaTime;
    //            bulletTimer5 += item.IsAttackTimeReduced ? Time.deltaTime * (float)1.5f : Time.deltaTime;

    //            if (bulletTimer5 >= bulletDatasList[5].attackInterval)
    //            {
    //                bulletTimer5 = 0;

    //                bulletGenerator5.PrepareGenerateBullet(direction);

    //                //Debug.Log("氷発射");
    //            }
    //        }

    //        yield return null;
    //    }
    //}

    /// <summary>
    /// 攻撃
    /// </summary>
    //private void Attack()
    //{
    //    //Bullet bullet = Instantiate(bulletPrefab, transform);
    //    //bullet.transform.SetParent(temporaryObjectsPlace);
    //    //bullet.Shoot(direction);

    //    //上の処理をまとめる
    //    //bulletGenerator.PrepareGenerateBullet(direction);
    //    //StartCoroutine(bulletGenerator2.PrepareGenerateBullet());
    //    //bulletGenerator3.PrepareGenerateBullet(direction);
    //    //bulletGenerator5.PrepareGenerateBullet(direction);

    //    //Debug.Log("攻撃");
    //}

    /// <summary>
    /// HP更新　
    /// </summary>
    public void UpdateHp(int value)
    {
        //ゲーム終了(isGameUpがtrue)の場合は処理しない。この処理を書かないと、リザルト画面時、敵とぶつかった時に以下の処理が繰り返されてしまう
        if (gameManager.IsGameUp)
        {
            return;
        }

        //レベル25以上の時受けるダメージの量+1
        if (charaLevel >= 25 && value < 0)
        {
            value -= 1;
        }

        hp = Mathf.Clamp(hp += value, 0, maxHp);

        uiManager.UpdateHpGauge();

        if (hp <= 0)
        {
            //TODO 死亡アニメ

            //TODO アニメが終わってから

            //リバイブを持っていたら生き返る
            if (item.HasReviveItem)
            {
                item.Revive();

                return;
            }

            gameManager.IsGameUp = true;

            resultPop.ShowPopUp();
        }
    }

    /// <summary>
    /// Expを加算する
    /// </summary>
    /// <param name="exp"></param>
    public void AddExp(int exp)
    {
        totalExp += exp;

        JudgeIsReadyToLevelUp();
    }

    /// <summary>
    /// レベルアップできるかどうかを判断し、それに伴った処理を実行する
    /// </summary>
    private void JudgeIsReadyToLevelUp()
    {
        if (totalExp >= needExpForLevelUp)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// レベルアップ処理
    /// </summary>
    private void LevelUp()
    {
        //Expの値をリセットする
        totalExp = 0;

        charaLevel++;

        //Debug.Log($"現在のレベル：{charaLevel}");

        //次のレベルアップに必要なExpを増やす(レベルが上がるにつれて、必要なExpも増える)
        needExpForLevelUp += addPoint * (charaLevel - 1);

        //キャラのレベルのUI更新
        uiManager.UpdateDisplayCharaLevel();

        levelupPop.ShowPopUp();  //bulletDatasList

        //レベルに応じてバレット4の生成時間を短縮
        //bulletGenerator4.SetAttackIntervalByLevel();

        //レベルに応じて、新しい敵の生成と生成のインターバル時間の短縮を行う
        //if (charaLevel == 5)
        //{
        //    StartCoroutine(enemyGenerator1.GenerateEnemy(this));
        //}
        //if (charaLevel == 8)
        //{
        //    StartCoroutine(enemyGenerator2.GenerateEnemy(this));
        //}

        //新しい敵用のジェネレーターを生成するか判定
        CheckCreateEnemyGenerator();

        //if (charaLevel == 11 || charaLevel == 14 || charaLevel == 17)
        //{
        //    enemyGenerator.GenerateInterval -= 0.5f;
        //    enemyGenerator1.GenerateInterval -= 0.5f;
        //    enemyGenerator2.GenerateInterval -= 0.5f;
        //}
        //if (charaLevel == 19)
        //{
        //    enemyGenerator1.GenerateInterval -= 0.5f;
        //    enemyGenerator2.GenerateInterval -= 0.5f;
        //}
        //if (charaLevel == 21)
        //{
        //    enemyGenerator2.GenerateInterval -= 0.5f;
        //}

        //TODO インターバル関係
        //インターバル値の更新については、各Generator内のCheckGenerateIntervalメソッドで判定を行う
        //foreach (var enemyGenerator in enemyGeneratorList)
        //{
        //    enemyGenerator.CheckGenerateInterval(charaLevel);
        //}

        //上記の処理をLINQで記述
        enemyGeneratorList.ForEach(enemyGenerator => enemyGenerator.CheckGenerateInterval(charaLevel));
    }

    /// <summary>
    /// 新しい敵のジェネレーターを生成するか判定
    /// </summary>
    private void CheckCreateEnemyGenerator()
    {
        //GenerateData generateData = null;

        ////判定
        //if (charaLevel == 5)
        //{
        //    generateData = DataBaseManager.instance.generateDataSO.GetGenerateData(1);
        //}
        //else if (charaLevel == 8)
        //{
        //    generateData = DataBaseManager.instance.generateDataSO.GetGenerateData(2);
        //}

        ////該当がある場合のみ
        //if (generateData)
        //{
        //    //新しい敵の生成を開始
        //    enemyGenerator.SetUpEnemyGenerator(generateData, gameManager, this);
        //}

        //上記のリファクタリング(スクリプタブルオブジェクトを修正すれば、自動的にチェックしてくれる)
        foreach (GenerateData generateData in DataBaseManager.instance.generateDataSO.generateDataList)
        {
            if (generateData.openCharaLevel == charaLevel)
            {
                //EnemyGenerator生成
                EnemyGeneratorObjectPool enemyGenerator = Instantiate(generateData.generatorPrefab, enemyGeneratorSetTran);

                //enemyGenerator.transform.localPosition = generateData.generatorTran.position;

                //生成する敵の情報を設定
                enemyGenerator.SetUpEnemyGenerator(generateData, gameManager, this);

                enemyGeneratorList.Add(enemyGenerator);

                //Break;で抜けてもいい
            }
        }

    }

    /// <summary>
    /// バレットのデータをリスト化
    /// </summary>
    //private void CreateBulletDatasList()
    //{
    //    foreach (var bulletData in DataBaseManager.instance.bulletDataSO.bulletDataList)
    //    {
    //        bulletDatasList.Add(bulletData);
    //    }
    //}

    /// <summary>
    /// アタッチされているBulletGeneratorを各変数に入れる
    /// </summary>
    //public void AssainBulletGenerators()
    //{
    //    TryGetComponent(out bulletGenerator);
    //    TryGetComponent(out bulletGenerator1);
    //    TryGetComponent(out bulletGenerator2);
    //    TryGetComponent(out bulletGenerator3);
    //    TryGetComponent(out bulletGenerator4);
    //    TryGetComponent(out bulletGenerator5);

    //    //Debug.Log("AssainBulletGeneratorsが動きました");
    //}

    /// <summary>
    /// 武器の追加
    /// </summary>
    /// <param name="selectedBulletData"></param>
    public void AttatchBulletGenerator(BulletDataSO.BulletData selectedBulletData)  //selectedBulletData = 選ばれたバレットのデータ
    {
        //GeneratorType(enum)を文字列に変換し、その名前と同じType(クラス)が存在するか判定
        Type type = Type.GetType(selectedBulletData.generatorType.ToString());  //using System;を追加する

        //該当するクラスがないなら処理を終了
        if (type == null) 
        {
            Debug.Log($"{selectedBulletData.generatorType.ToString()} が見つかりません");

            return;
        }

        //GeneratorType(enum)に該当するクラスが見つかったのでアタッチして設定する
        BulletGeneratorBase bulletGenerator = gameObject.AddComponent(type) as BulletGeneratorBase;  //as 〇〇で、〇〇にキャスト

        bulletGenerator.SetUpBulletGenerator(this, selectedBulletData);

        AttatchedBulletGeneratorList.Add(bulletGenerator);

        Debug.Log($"{selectedBulletData.generatorType.ToString()} をアタッチします");
    }
}
