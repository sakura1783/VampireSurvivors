using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenerateData
{
    public int id;
    public int openCharaLevel;  //キャラのレベルがこの値に達している場合、Generatorを追加
    public float generateInterval;
    public EnemyController enemyPrefab;
    public EnemyGeneratorObjectPool generatorPrefab;

    public UpgradeData[] upgradeDatas;


    /// <summary>
    /// レベルアップ用のデータクラス
    /// </summary>
    [System.Serializable]
    public class UpgradeData
    {
        public int level;  //キャラのレベルがこの値に達したらoffsetInterval分だけインターバル時間を短縮
        public float offsetInterval;
    }
}


[CreateAssetMenu(fileName = "GenerateDataSO", menuName = "Create GenerateDataSO")]
public class GenerateDataSO : ScriptableObject
{
    public List<GenerateData> generateDataList = new();

    public GenerateData GetGenerateData(int searchNo)
    {
        return generateDataList.Find(data => data.id == searchNo);
    }
}
