using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float gameTime;
    public float GameTime => gameTime;

    //[SerializeField] private float remainingTime;  //残り時間
    //public float RemainingTime => remainingTime;

    [SerializeField] private CharaController charaController;

    //[SerializeField] private EnemyGenerator enemyGenerator;
    [SerializeField] private EnemyGeneratorObjectPool enemyGenerator;

    [SerializeField] private MapManager mapManager;
    public MapManager MapManager => mapManager;

    //[SerializeField] private CharaManager charaManager;

    [SerializeField] private Cannon cannon0;
    [SerializeField] private Cannon cannon1;

    [SerializeField] private Transform shurikenPlace;

    [SerializeField] private ItemPopUp itemPop;

    [SerializeField] private TreasureChestGenerator treasureChestGenerator;

    [SerializeField] private UIManager uiManager;

    [SerializeField] private int scorePerSecond;  //1秒あたりのスコア加算ポイント

    [SerializeField] private TitlePopUp titlePop;

    [SerializeField] private NameEntryPopUp nameEntryPop;

    //private bool isDisplayPopUp = false;  //ポップアップ表示中かどうか
    ////public bool IsDisplayPopUp { get; set; }
    //public bool IsDisplayPopUp
    //{
    //    get { return isDisplayPopUp; }
    //    //get => isDisplayPopUp;
    //    set { isDisplayPopUp = value; }
    //    //set => isDisplayPopUp = value;  //setするときに2行以上処理を書くときは省略は使えないので注意
    //}

    //private bool isDisplayTitlePopUp = true;
    //public bool IsDisplayTitlePopUp
    //{
    //    get => isDisplayTitlePopUp; set => isDisplayTitlePopUp = value;
    //}

    //private bool isDisplayResultPopUp = false;
    //public bool IsDisplayResultPopUp
    //{
    //    get => isDisplayResultPopUp; set => isDisplayResultPopUp = value;
    //}

    //上記の代わりにTimeScale停止用の変数と弾や敵の生成停止用の変数を作る
    private bool isTimePaused = false;  //TimeScaleを0にするかどうか(すでに生成されている弾などの挙動を一時停止)
    public bool IsTimePaused
    {
        get => isTimePaused;
        set => isTimePaused = value;
    }

    private bool isProcessingPaused = false;  //ゲーム内で行われている処理を停止するかどうか(まだ生成されていない敵や弾の生成やプレイ時間の計測の一時停止)
    public bool IsProcessingPaused
    {
        get => isProcessingPaused;
        set => isProcessingPaused = value;
    }

    private int killEnemyCount;
    public int KillEnemyCount => killEnemyCount;

    private int totalScore;
    public int TotalScore => totalScore;

    private float scoreTimer;  //時間経過でもスコアを加算する

    private string playerName;
    public string PlayerName
    {
        get => playerName; set => playerName = value;
    }

    private bool isGameUp = false;
    public bool IsGameUp { get => isGameUp; set => isGameUp = value; }


    void Start()
    {
        titlePop.SetUpTitlePopUp();

        nameEntryPop.SetUpNameEntryPopUp();

        //remainingTime = gameTime;

        mapManager.JudgeClimbedStairs();

        //各初期設定
        charaController.SetUpCharaController();
        //charaManager.SetUpCharaManager();

        itemPop.SetUpItemPopUp();

        treasureChestGenerator.SetUpTreasureChestGenerator();

        //敵の生成開始
        //StartCoroutine(enemyGenerator.GenerateEnemy(charaController));

        //大砲の攻撃開始
        StartCoroutine(cannon0.PrepareGenerateBullet());
        StartCoroutine(cannon1.PrepareGenerateBullet());
    }

    void Update()
    {
        if (!isProcessingPaused)
        {
            //残り時間の更新
            gameTime += Time.deltaTime;

            //残り時間表示のUIを更新
            uiManager.UpdateDisplayGameTime();

            //if (remainingTime <= 0)
            //{
            //    //ゲーム終了の処理
            //}

            //時間経過でもスコアを加算(1秒ごとに10加算)
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= 1)
            {
                scoreTimer = 0;

                AddScore(scorePerSecond);
            }
        }

        //ポップアップ表示中は物理演算で動いているゲームオブジェクト(例えばバレットなど)の動きを一時停止する
        if (isTimePaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// 倒した敵の数をカウントアップし、同時にUIも更新
    /// </summary>
    public void AddKillEnemyCount()
    {
        killEnemyCount++;

        uiManager.UpdateDisplayKillEnemyCount(killEnemyCount);
    }

    /// <summary>
    /// スコアを加算し、UIも更新
    /// </summary>
    public void AddScore(int point)
    {
        totalScore += point;

        uiManager.UpdateDisplayTotalScore(totalScore);
    }

    /// <summary>
    /// playersDataディクショナリに値を追加する
    /// </summary>
    //public void AddToPlayersDataDic()
    //{
    //    //スコアが3位以内ならディクショナリに追加
    //    if (totalScore >= GameData.instance.thirdScore)
    //    {
    //        GameData.instance.playersData.Add(playerName, totalScore);
    //    }
    //}

    /// <summary>
    /// playersDataリストに値を追加する
    /// </summary>
    public void AddToPlayersDataList()
    {
        //セーブしておいたthirdScoreをロード
        GameData.instance.thirdScore = PlayerPrefs.GetInt("ThirdScore_Key");

        //スコアが3位以内ならリストに値を追加
        if (totalScore >= GameData.instance.thirdScore)
        {
            GameData.instance.playersDataList.Add((playerName, totalScore));  //タプルを使っているのでカッコを忘れないようにする。タプルは(型1 値1, 型2 値2)という風に記述するため

            Debug.Log("スコアを追加します");
        }
    }

    /// <summary>
    /// ハイスコアを保存する
    /// </summary>
    public void SaveHighScore()
    {
        //TODO 名前か何かで指定しないと他のプレイヤーの記録が自分のハイスコアに置き換わってしまうのではないか

        GameData.instance.highScore = PlayerPrefs.GetInt("HighScore_Key");

        if (totalScore > GameData.instance.highScore)
        {
            GameData.instance.highScore = totalScore;

            PlayerPrefs.SetInt("HighScore_Key", GameData.instance.highScore);
            PlayerPrefs.SetInt("KillEnemyCount_Key", killEnemyCount);
            PlayerPrefs.SetInt("SurvivedTime_Key", (int)gameTime);
            PlayerPrefs.Save();
        }
    }
}
