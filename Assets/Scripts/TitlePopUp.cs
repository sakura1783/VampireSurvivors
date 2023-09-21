using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TitlePopUp : MonoBehaviour
{
    [SerializeField] private CanvasGroup popupCanvasGroup;
    //[SerializeField] private CanvasGroup lblTapPromptCanvasGroup;

    [SerializeField] private Button btnGameStart;
    [SerializeField] private Button btnEditName;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private NameEntryPopUp nameEntryPop;


    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpTitlePopUp()
    {
        SetUpButtons();

        //lblTapPromptCanvasGroup.DOFade(0, 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);  //ループ処理があるのでSetLinkを書く

        //Debug.Log("SetUpTitlePopupが動きました");
    }

    /// <summary>
    /// 各ボタンの設定
    /// </summary>
    private void SetUpButtons()
    {
        btnGameStart.onClick.AddListener(OnClickBtnGameStart);
        btnEditName.onClick.AddListener(OnClickBtnEditName);
    }

    /// <summary>
    /// btnGameStartを押した際の処理
    /// </summary>
    private void OnClickBtnGameStart()
    {
        HidePopUp();
    }

    /// <summary>
    /// btnEditNameを押した際の処理
    /// </summary>
    private void OnClickBtnEditName()
    {
        //ポップアップのボタン押下反応を無くす
        popupCanvasGroup.blocksRaycasts = false;

        nameEntryPop.ShowPopUp();
    }

    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    public void HidePopUp()
    {
        popupCanvasGroup.blocksRaycasts = false;
        popupCanvasGroup.DOFade(0, 1f).SetEase(Ease.Linear).OnComplete(() => gameManager.IsDisplayTitlePopUp = false);  //ここにはループの処理はないためSetLinkは書かなくてよい
    }
}
