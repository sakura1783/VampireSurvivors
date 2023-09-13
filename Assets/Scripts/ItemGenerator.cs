using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private GameObject treasureChest;

    [SerializeField] private Transform temporaryObjectsPlace;


    /// <summary>
    /// アイテム生成準備
    /// </summary>
    public void PrepareGenerateItem(EnemyController enemyController)
    {
        if (Random.Range(0, 100) < 15)
        {
            GenerateItem(enemyController);
        }
    }

    /// <summary>
    /// アイテム(宝箱)生成
    /// </summary>
    /// <param name="enemyController"></param>
    private void GenerateItem(EnemyController enemyController)
    {
        GameObject chest = Instantiate(treasureChest, enemyController.transform.position, Quaternion.identity);

        chest.transform.SetParent(temporaryObjectsPlace);
    }
}
