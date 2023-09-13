using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CharaController charaController;

    [SerializeField] private EnemyGenerator enemyGenerator;

    [SerializeField] private MapManager mapManager;

    //[SerializeField] private CharaManager charaManager;

    [SerializeField] private Cannon cannon0;
    [SerializeField] private Cannon cannon1;

    [SerializeField] private Transform shurikenPlace;

    private bool isDisplayPopUp;  //ポップアップ表示中かどうか
    public bool IsDisplayPopUp { get; set; }


    void Start()
    {
        mapManager.JudgeClimbedStairs();

        charaController.SetUpCharaController();
        //charaManager.SetUpCharaManager();

        //敵の生成開始
        StartCoroutine(enemyGenerator.GenerateEnemy(charaController));

        //大砲の攻撃開始
        StartCoroutine(cannon0.PrepareGenerateBullet());
        StartCoroutine(cannon1.PrepareGenerateBullet());
    }

    void Update()
    {
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
