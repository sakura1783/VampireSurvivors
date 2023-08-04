using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CharaController charaController;

    [SerializeField] private EnemyGenerator enemyGenerator;

    [SerializeField] private MapManager mapManager;

    void Start()
    {
        mapManager.JudgeClimbedStairs();

        charaController.SetUpCharaController();

        StartCoroutine(enemyGenerator.GenerateEnemy(charaController));
    }
}
