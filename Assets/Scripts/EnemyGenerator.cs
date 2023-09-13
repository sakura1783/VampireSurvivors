using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private float generateInterval;

    [SerializeField] private int maxGenerateCount;

    [SerializeField] private EnemyController enemyPrefab;

    //[SerializeField] private LevelupPopUp levelupPop;

    [SerializeField] private GameManager gameManager;


    /// <summary>
    /// 敵の生成
    /// </summary>
    /// <returns></returns>
    public IEnumerator GenerateEnemy(CharaController charaController)
    {
        int generateCount = 0;

        while (generateCount < maxGenerateCount)
        {
            //ポップアップ表示中の場合、処理をスキップしてwhile文の最初に戻り、ポップアップが非表示になるまで新しい敵を生成しない
            if (gameManager.IsDisplayPopUp)
            {
                yield return null;  //これを書かないと無限ループになる。注意

                continue;
            }

            yield return new WaitForSeconds(generateInterval);

            EnemyController enemy = Instantiate(enemyPrefab, transform);

            GameData.instance.enemiesList.Add(enemy);

            enemy.SetUpEnemyController(charaController);

            generateCount++;

            Debug.Log($"現在の敵の生成数：{generateCount}体");
        }

        Debug.Log("全ての敵を生成しました");
    }
}
