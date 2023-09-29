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

    [SerializeField] private Text txt1stName;
    public Text Txt1stName => txt1stName;
    [SerializeField] private Text txt1stScore;
    public Text Txt1stScore => txt1stScore;
    [SerializeField] private Text txt2ndName;
    public Text Txt2ndName => txt2ndName;
    [SerializeField] private Text txt2ndScore;
    public Text Txt2ndScore => txt2ndScore;
    [SerializeField] private Text txt3rdName;
    public Text Txt3rdName => txt3rdName;
    [SerializeField] private Text txt3rdScore;
    public Text Txt3rdScore => txt3rdScore;


    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpTitlePopUp()
    {
        SetUpButtons();

        //lblTapPromptCanvasGroup.DOFade(0, 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);  //ループ処理があるのでSetLinkを書く

        //Debug.Log("SetUpTitlePopupが動きました");

        GameData.instance.PrepareSetUpGameData(this);

        //ランキングの読み込み(シーン遷移によってテキスト(UI)が初期状態に戻ってしまうのでここでランキングを読み込んで表示する)
        GameData.instance.LoadRanking();

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
