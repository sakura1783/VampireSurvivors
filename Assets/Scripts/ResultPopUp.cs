using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class ResultPopUp : MonoBehaviour
{
    [SerializeField] private GameObject resultPopUpObj;

    [SerializeField] private Button btnToTitle;

    [SerializeField] private Text txtScore;
    [SerializeField] private Text txtKillEnemyCount;
    [SerializeField] private Text txtSurvivedTime;

    [SerializeField] private CanvasGroup lblTapPromptCanvasGroup;

    [SerializeField] private GameManager gameManager;


    /// <summary>
    /// 初期設定
    /// </summary>
    //private void SetUpResultPopUp()
    //{
    //    SetUpButtons();

    //    //点滅表示
    //    lblTapPromptCanvasGroup.DOFade(0, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
    //}

    /// <summary>
    /// 各ボタンの設定
    /// </summary>
    private void SetUpButtons()
    {
        btnToTitle.onClick.AddListener(OnClickBtnToTitle);
    }

    /// <summary>
    /// btnToTitleを押した際の処理
    /// </summary>
    private void OnClickBtnToTitle()
    {
        //3位以内の場合、プレイヤーの名前とスコアをplayersDataListに追加
        gameManager.AddToPlayersDataList();

        //Debug.Log("1");

        //playersDataListの整理と並び替え
        GameData.instance.OrganizePlayersDataList();

        //Debug.Log("2");

        gameManager.IsProcessingPaused = false;

        //シーン遷移
        TransitionManager.instance.PrepareLoadNextScene();

        //Debug.Log("3");
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    public void ShowPopUp()
    {
        gameManager.IsProcessingPaused = true;

        Sequence seqence = DOTween.Sequence();

        seqence.Append(gameObject.transform.DOLocalMoveY(0, 1.5f));
        seqence.AppendInterval(1.5f).OnComplete(() =>
        {
            //BGM再生
            AudioManager.instance.PreparePlayBGM(BgmType.Result);

            SetPlayerResult();
        });  //ポップアップが画面にスライドするのを待ってからSetPlayerResultメソッドを実行
    }

    /// <summary>
    /// プレイ結果をポップアップに表示
    /// </summary>
    private void SetPlayerResult()
    {
        SetUpButtons();

        Sequence seqence = DOTween.Sequence();

        seqence.Append(txtScore.DOCounter(0, gameManager.TotalScore, 1f).SetEase(Ease.Linear));
        seqence.AppendInterval(1f);

        seqence.Append(txtKillEnemyCount.DOCounter(0, gameManager.KillEnemyCount, 1f).SetEase(Ease.Linear));
        seqence.AppendInterval(1f);

        seqence.Append(txtSurvivedTime.DOCounter(0, (int)gameManager.GameTime, 1f).SetEase(Ease.Linear));
        seqence.AppendInterval(1f).OnComplete(() =>
        {
            //点滅表示
            lblTapPromptCanvasGroup.DOFade(1, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);

            btnToTitle.image.raycastTarget = true;  //<= 全ての処理が終わってからボタン押下反応をアクティブにする
        });  
    }
}
