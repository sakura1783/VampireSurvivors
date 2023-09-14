using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreasureChestGenerator : MonoBehaviour
{
    [SerializeField] private ItemPopUp itemPop;

    [SerializeField] private TreasureChest treasureChestPrefab;

    [SerializeField] private Transform temporaryObjectsPlace;


    /// <summary>
    /// 宝箱生成
    /// </summary>
    public void GenerateTreasureChest(Transform enemyTran)
    {
        TreasureChest treasureChest = Instantiate(treasureChestPrefab, enemyTran.position, Quaternion.identity);

        treasureChest.transform.SetParent(temporaryObjectsPlace);

        treasureChest.SetUpTreasureChest(itemPop);

        //ランダムにアイテムを決定し、ポップアップにアイテムの情報を渡す
        GetRandomItem();
    }

    [System.Serializable]
    public class ItemData
    {
        public ItemType itemType;
        public int maxGenerateCount;
        public int generateRate;  //重み付け
    }

    [SerializeField] private List<ItemData> itemDataList = new();

    [System.Serializable]
    public class GenerateItem
    {
        public int itemNo;  //Key
        public int generateCount;  //Value
    }

    private Dictionary<ItemType, int> generateCountByItemType = new();

    //生成する全アイテムの合計数を算出したプロパティ
    public int MaxGenerateItemCount => itemDataList.Sum(itemData => itemData.maxGenerateCount);

    private int totalGenerateRate = 0;

    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpTreasureChestGenerator()
    {
        //各アイテムの生成数を初期化
        InitializeGenerateCounts();
    }

    /// <summary>
    /// 各アイテムの生成数を初期化
    /// </summary>
    private void InitializeGenerateCounts()
    {
        //データをクリア
        generateCountByItemType.Clear();

        //itemDataListを走査(スキャン)しながら各要素(ItemData)のenemyNoをキー、Valueを0として初期設定
        foreach (var itemData in itemDataList)
        {
            generateCountByItemType[itemData.itemType] = 0;
        }

        Debug.Log("アイテムの生成数初期化");
    }

    /// <summary>
    /// 最大生成数を超えていないアイテムの生成確率の合計値を算出
    /// </summary>
    private void CalculateTotalGenerateRate()
    {
        //foreachを使う場合、変数を初期化してから使う(前の値がクリアされないため)
        totalGenerateRate = 0;

        foreach (var itemData in itemDataList)
        {
            //TryGetValue(取得したいKey, 取得できた場合にValueの値を格納するための変数をoutを使用して指定)
            if (generateCountByItemType.TryGetValue(itemData.itemType, out int generateCount) && generateCount < itemData.maxGenerateCount)
            {
                totalGenerateRate += itemData.generateRate;
            }
        }

        Debug.Log($"生成合計値 : {totalGenerateRate}");  //TODO 確認したらコメントアウト
    }

    /// <summary>
    /// ランダムに決定されたアイテムをポップアップに表示
    /// </summary>
    private void GetRandomItem()
    {
        //生成可能なアイテムの生成率の再計算
        CalculateTotalGenerateRate();

        int randomNo = Random.Range(0, totalGenerateRate);
        Debug.Log($"ランダムな値：{randomNo}");  //TODO 確認したらコメントアウト

        //取得したランダムな値をGetRandomItemTypeの引数に渡して、どのアイテムを生成するか、アイテムの番号を取得
        ItemType itemType = GetRandomItemType(randomNo);

        if (itemType == ItemType.None)
        {
            Debug.Log("アイテムがありません");
        }

        //生成した種類のアイテムだけカウントアップ
        generateCountByItemType[itemType]++;
        Debug.Log($"アイテムの種類は{itemType}です");  //TODO 確認したらコメントアウト

        //アイテムの情報をポップアップに渡す
        itemPop.SetRandomItemDetail(itemType);
    }

    /// <summary>
    /// ランダムにアイテムを決定
    /// </summary>
    /// <param name="randomNo"></param>
    /// <returns></returns>
    private ItemType GetRandomItemType(int randomNo)
    {
        int cumulativeChance = 0;  //重み付け用の値。cumulative = 累積的な

        foreach (var itemData in itemDataList)
        {
            //itemDataのitemNoとgenerateCountByItemTypeディクショナリのキーを対応させることで、正しいアイテムの生成数を取得し、最大数と比較
            if (generateCountByItemType[itemData.itemType] < itemData.maxGenerateCount)
            {
                //生成数を超えていない場合に限り、重み付け用の値を加算
                cumulativeChance += itemData.generateRate;

                if (randomNo < cumulativeChance)
                {
                    return itemData.itemType;
                }
            }
        }

        return ItemType.None;
    }
}
