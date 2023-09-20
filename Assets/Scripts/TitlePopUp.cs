using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TitlePopUp : MonoBehaviour
{
    [SerializeField] private CanvasGroup popupCanvasGroup;
    [SerializeField] private CanvasGroup lblTapPromptCanvasGroup;

    [SerializeField] private Button btnGameStart;

    [SerializeField] private GameManager gameManager;


    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpTitlePopUp()
    {
        SetUpButtons();

        lblTapPromptCanvasGroup.DOFade(0, 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        Debug.Log("SetUpTitlePopupが動きました");
    }

    /// <summary>
    /// 各ボタンの設定
    /// </summary>
    private void SetUpButtons()
    {
        btnGameStart.onClick.AddListener(OnClickBtnGameStart);
    }

    /// <summary>
    /// btnGameStartを押した際の処理
    /// </summary>
    private void OnClickBtnGameStart()
    {
        HidePopUp();
    }

    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    public void HidePopUp()
    {
        popupCanvasGroup.blocksRaycasts = false;
        popupCanvasGroup.DOFade(0, 1f).SetEase(Ease.Linear).SetLink(gameObject);

        gameManager.IsDisplayPopUp = false;
    }
}
