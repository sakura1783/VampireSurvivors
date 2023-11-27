using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

public class TitlePopUp : MonoBehaviour
{
    [SerializeField] private CanvasGroup popupCanvasGroup;
    //[SerializeField] private CanvasGroup lblTapPromptCanvasGroup;

    [SerializeField] private Button btnGameStart;
    [SerializeField] private Button btnEditName;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private NameEntryPopUp nameEntryPop;

    [SerializeField] private Text txt1stName;
    //public Text Txt1stName => txt1stName;
    [SerializeField] private Text txt1stScore;
    //public Text Txt1stScore => txt1stScore;
    [SerializeField] private Text txt2ndName;
    //public Text Txt2ndName => txt2ndName;
    [SerializeField] private Text txt2ndScore;
    //public Text Txt2ndScore => txt2ndScore;
    [SerializeField] private Text txt3rdName;
    //public Text Txt3rdName => txt3rdName;
    [SerializeField] private Text txt3rdScore;
    //public Text Txt3rdScore => txt3rdScore;

    //自己ベスト保存用
    [SerializeField] private Text txtHighScore;
    [SerializeField] private Text txtKillEnemyCount;
    [SerializeField] private Text txtSurvivedTime;


    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpTitlePopUp()
    {
        gameManager.IsProcessingPaused = true;

        SetUpButtons();

        //lblTapPromptCanvasGroup.DOFade(0, 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);  //ループ処理があるのでSetLinkを書く

        //Debug.Log("SetUpTitlePopupが動きました");

        //GameData.instance.PrepareSetUpGameData(this);

        //ランキングの読み込み(シーン遷移によってテキスト(UI)が初期状態に戻ってしまうのでここでランキングを読み込んで表示する)
        LoadRanking();

        LoadHighScore();

        //BGM再生
        AudioManager.instance.PreparePlayBGM(BgmType.Title);
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
        AudioManager.instance.PlaySE(SeType.GameStartBtn);

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
        popupCanvasGroup.DOFade(0, 1f).SetEase(Ease.Linear).OnComplete(() =>  //ここにはループの処理はないためSetLinkは書かなくてよい
        {
            gameManager.IsProcessingPaused = false;

            //BGM再生
            AudioManager.instance.PreparePlayBGM(BgmType.Battle);
        });  
    }

    /// <summary>
    /// ランキング読み込み
    /// </summary>
    public void LoadRanking()
    {
        //セーブしておいたplayersDataリストをロード
        List<(string playerName, int score)> playersDataList = PlayerPrefsUtility.LoadList<(string, int)>("PlayersDataList_Key");

        //何回foreach文が回ったか。この値でどの順位にどの値を入れるかを決める
        int count = 0;

        //スコアが高い順にリストを並び替え
        var sortedPlayersDataList = playersDataList.OrderByDescending(data => data.score);

        foreach (var playerData in sortedPlayersDataList)
        {
            //スコアが0以上であればタイトル画面の対応するランキングに表示
            if (playerData.score >= 0)
            {
                if (count == 0)
                {
                    txt1stName.text = playerData.playerName;
                    txt1stScore.text = playerData.score.ToString();
                }
                if (count == 1)
                {
                    txt2ndName.text = playerData.playerName;
                    txt2ndScore.text = playerData.score.ToString();
                }
                if (count == 2)
                {
                    txt3rdName.text = playerData.playerName;
                    txt3rdScore.text = playerData.score.ToString();

                    Debug.Log("ランキングを読み込みました");

                    //ランキングが全て埋まったら抜ける
                    return;
                }
            }

            count++;
        }
    }

    /// <summary>
    /// ハイスコアを読み込む
    /// </summary>
    private void LoadHighScore()
    {
        txtHighScore.text = PlayerPrefs.GetInt("HighScore_Key").ToString();
        txtKillEnemyCount.text = PlayerPrefs.GetInt("KillEnemyCount_Key").ToString() + "体";
        txtSurvivedTime.text = PlayerPrefs.GetInt("SurvivedTime_Key").ToString() + "秒";
    }
}
