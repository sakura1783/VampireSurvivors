using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameEntryPopUp : MonoBehaviour
{
    [SerializeField] private CanvasGroup popUpCanvasGroup;
    [SerializeField] private CanvasGroup titlePopCanvasGroup;

    [SerializeField] private InputField inputField;

    [SerializeField] private Button btnEntry;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private TitlePopUp titlePop;


    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpNameEntryPopUp()
    {
        SetUpButtons();
    }

    /// <summary>
    /// 各ボタンの設定
    /// </summary>
    private void SetUpButtons()
    {
        btnEntry.onClick.AddListener(OnClickBtnEntry);
    }

    /// <summary>
    /// btnCloseを押した際の処理
    /// </summary>
    private void OnClickBtnEntry()
    {
        HidePopUp();

        //プレイヤー名を取得し、保存
        GetPlayerName();
    }

    /// <summary>
    /// 入力されたプレイヤーの名前を取得する
    /// </summary>
    public void GetPlayerName()
    {
        string playerName = inputField.text;
        Debug.Log($"プレイヤー名：{playerName}");

        //プレイヤー名を一時的に保存
        gameManager.PlayerName = playerName;
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    public void ShowPopUp()
    {
        popUpCanvasGroup.alpha = 1;
        popUpCanvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    public void HidePopUp()
    {
        popUpCanvasGroup.blocksRaycasts = false;
        popUpCanvasGroup.alpha = 0;

        titlePopCanvasGroup.blocksRaycasts = true;
    }
}
