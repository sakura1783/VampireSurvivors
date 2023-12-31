using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopUp : MonoBehaviour
{
    [SerializeField] private CanvasGroup popUpCanvasGroup;
    [SerializeField] private CanvasGroup step1CanvasGroup;
    [SerializeField] private CanvasGroup step2CanvasGroup;

    [SerializeField] private Button btnNext;
    [SerializeField] private Button btnGet;

    [SerializeField] private Text txtItemName;
    [SerializeField] private Text txtDescliption;

    [SerializeField] private Image imgItem;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private Item item;

    //private List<ItemDataSO.ItemData> itemDatasList = new();

    private ItemDataSO.ItemData selectedItemData;  //選ばれたアイテムのItemData

    private GameObject treasureChestObj;


    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpItemPopUp()
    {
        popUpCanvasGroup.alpha = 0;

        //ボタンを押せなく(反応させなく)する
        popUpCanvasGroup.blocksRaycasts = false;

        step1CanvasGroup.blocksRaycasts = true;
        step2CanvasGroup.blocksRaycasts = false;

        //アイテムのデータをリスト化
        //CreateItemDatasList();

        SetUpButtons();
    }

    /// <summary>
    /// 各ボタンの設定
    /// </summary>
    private void SetUpButtons()
    {
        btnNext.onClick.AddListener(OnClickBtnNext);
        btnGet.onClick.AddListener(OnClickBtnGet);
    }

    /// <summary>
    /// btnNextを押した際の処理
    /// </summary>
    private void OnClickBtnNext()
    {
        step1CanvasGroup.alpha = 0;
        step1CanvasGroup.blocksRaycasts = false;

        step2CanvasGroup.alpha = 1;
        step2CanvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// btnGetを押した際の処理
    /// </summary>
    private void OnClickBtnGet()
    {
        HidePopUp();

        //アイテムの効果を発動する
        item.ApplyItemEffect(selectedItemData.itemType);
    }

    /// <summary>
    /// ポップアップを表示
    /// </summary>
    public void ShowPopUp(GameObject treasureChestObj)
    {
        gameManager.IsTimePaused = true;
        gameManager.IsProcessingPaused = true;

        popUpCanvasGroup.alpha = 1;
        popUpCanvasGroup.blocksRaycasts = true;

        this.treasureChestObj = treasureChestObj;
    }

    /// <summary>
    /// ポップアップを非表示
    /// </summary>
    public void HidePopUp()
    {
        gameManager.IsTimePaused = false;
        gameManager.IsProcessingPaused = false;

        InitializePopUp();

        //ポップアップを閉じるタイミングで宝箱も破棄
        Destroy(treasureChestObj);
    }

    /// <summary>
    /// ポップアップの初期化(2回目以降の表示に備える)
    /// </summary>
    private void InitializePopUp()
    {
        popUpCanvasGroup.alpha = 0;
        step1CanvasGroup.alpha = 1;
        step2CanvasGroup.alpha = 0;

        popUpCanvasGroup.blocksRaycasts = false;
        step1CanvasGroup.blocksRaycasts = true;
        step2CanvasGroup.blocksRaycasts = false;

        //初期化の際はDebugで順番を確認する。今回は値を設定した後に初期化してしまっていたのでポップアップの表示に問題が生じた
        //txtItemName.text = "";
        //txtDescliption.text = "";

        //Debug.Log("通過しました");
    }

    /// <summary>
    /// ランダムなアイテムの情報をポップアップに表示
    /// </summary>
    public void SetRandomItemDetail(ItemType itemType)
    {
        selectedItemData = GetItemDataByItemType(itemType);

        //Debug.Log($"アイテムの中身：{selectedItemData}");

        txtItemName.text = selectedItemData.itemType.ToString();
        txtDescliption.text = selectedItemData.descliption;
        imgItem.sprite = selectedItemData.itemSprite;
    }

    /// <summary>
    /// ItemTypeに対応するItemDataを取得
    /// </summary>
    public ItemDataSO.ItemData GetItemDataByItemType(ItemType itemType)
    {
        foreach (var itemData in DataBaseManager.instance.itemDataSO.itemDataList)
        {
            if (itemData.itemType == itemType)
            {
                return itemData;
            }
        }

        return null;
    }

    /// <summary>
    /// アイテムのデータをリスト化
    /// </summary>
    //private void CreateItemDatasList()
    //{
    //    foreach (var itemData in DataBaseManager.instance.itemDataSO.itemDataList)
    //    {
    //        itemDatasList.Add(itemData);
    //    }
    //}
}
