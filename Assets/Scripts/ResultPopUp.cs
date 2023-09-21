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
    public void SetUpResultPopUp()
    {
        SetUpButtons();

        //点滅表示
        lblTapPromptCanvasGroup.DOFade(0, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
    }

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
        //TODO フェードイン・フェードアウト

        //TODO isDisplayResultPopUpの切り替え

        //シーン遷移
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    public void ShowPopUp()
    {
        Sequence seqence = DOTween.Sequence();

        seqence.Append(gameObject.transform.DOLocalMoveY(0, 1.5f));
        seqence.AppendInterval(1.5f).OnComplete(() => SetPlayerResult());  //ポップアップが画面にスライドするのを待ってからSetPlayerResultメソッドを実行
    }

    /// <summary>
    /// プレイ結果をポップアップに表示
    /// </summary>
    private void SetPlayerResult()
    {
        Sequence seqence = DOTween.Sequence();

        seqence.Append(txtScore.DOCounter(0, gameManager.TotalScore, 1f).SetEase(Ease.InQuad));
        seqence.AppendInterval(1f);

        seqence.Append(txtKillEnemyCount.DOCounter(0, gameManager.KillEnemyCount, 1f).SetEase(Ease.InQuad));
        seqence.AppendInterval(1f);

        seqence.Append(txtSurvivedTime.DOCounter(0, (int)gameManager.GameTime, 1f).SetEase(Ease.InQuad));
        seqence.AppendInterval(1f).OnComplete(() => btnToTitle.image.raycastTarget = true);  //<= 全ての処理が終わってからボタン押下反応をアクティブにする
    }
}
