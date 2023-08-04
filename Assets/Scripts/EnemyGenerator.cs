using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private float generateInterval;

    [SerializeField] private int maxGenerateCount;

    [SerializeField] private EnemyController enemyPrefab;


    /// <summary>
    /// 敵の生成
    /// </summary>
    /// <returns></returns>
    public IEnumerator GenerateEnemy(CharaController charaController)
    {
        int generateCount = 0;

        while (generateCount < maxGenerateCount)
        {
            yield return new WaitForSeconds(generateInterval);

            EnemyController enemy = Instantiate(enemyPrefab, transform);

            enemy.SetUpEnemyController(charaController);

            generateCount++;

            Debug.Log($"現在の敵の生成数：{generateCount}体");
        }

        Debug.Log("全ての敵を生成しました");
    }
}
