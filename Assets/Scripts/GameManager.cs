using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float gameTime;  //1ゲームの時間

    [SerializeField] private float remainingTime;  //残り時間

    [SerializeField] private CharaController charaController;

    [SerializeField] private EnemyGenerator enemyGenerator;

    [SerializeField] private MapManager mapManager;

    //[SerializeField] private CharaManager charaManager;

    [SerializeField] private Cannon cannon0;
    [SerializeField] private Cannon cannon1;

    [SerializeField] private Transform shurikenPlace;

    [SerializeField] private ItemPopUp itemPop;

    [SerializeField] private TreasureChestGenerator treasureChestGenerator;

    [SerializeField] private bool isDisplayPopUp;  //ポップアップ表示中かどうか
    //public bool IsDisplayPopUp { get; set; }
    public bool IsDisplayPopUp
    {
        get { return isDisplayPopUp; }
        set { isDisplayPopUp = value; }
    }


    void Start()
    {
        remainingTime = gameTime;

        mapManager.JudgeClimbedStairs();

        //各初期設定
        charaController.SetUpCharaController();
        //charaManager.SetUpCharaManager();

        itemPop.SetUpItemPopUp();

        treasureChestGenerator.SetUpTreasureChestGenerator();

        //敵の生成開始
        StartCoroutine(enemyGenerator.GenerateEnemy(charaController));

        //大砲の攻撃開始
        StartCoroutine(cannon0.PrepareGenerateBullet());
        StartCoroutine(cannon1.PrepareGenerateBullet());
    }

    void Update()
    {
        //残り時間の更新
        remainingTime = Mathf.Clamp(remainingTime -= Time.deltaTime, 0, gameTime);

        if (remainingTime <= 0)
        {
            //TODO ゲーム終了の処理
        }

        //ポップアップ表示中は物理演算で動いているゲームオブジェクト(例えばバレットなど)の動きを一時停止する
        if (isDisplayPopUp)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
