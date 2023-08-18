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
}
