using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CharaController charaController;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private Text txtCharaLevel;
    [SerializeField] private Text txtGameTime;
    [SerializeField] private Text txtHp;
    [SerializeField] private Text txtScore;
    [SerializeField] private Text txtKillEnemyCount;

    [SerializeField] private Slider slider;
    
    /// <summary>
    /// キャラのレベルの表示を更新
    /// </summary>
    public void UpdateDisplayCharaLevel()
    {
        txtCharaLevel.text = "Lv." + charaController.charaLevel;
    }

    /// <summary>
    /// 残り時間の表示を更新
    /// </summary>
    public void UpdateDisplayGameTime()
    {
        txtGameTime.text = gameManager.GameTime.ToString("n1") + "秒";
    }

    /// <summary>
    /// HPゲージの更新
    /// </summary>
    public void UpdateHpGauge()
    {
        slider.DOValue((float)charaController.hp / (float)charaController.maxHp, 0.5f).SetLink(charaController.gameObject);  //float型にキャストしないとゲージが0になるので注意

        txtHp.text = charaController.hp + "/" + charaController.maxHp;
    }

    /// <summary>
    /// スコア表示の更新
    /// </summary>
    public void UpdateDisplayTotalScore(int totalScore)
    {
        txtScore.text = totalScore.ToString();
    }

    /// <summary>
    /// 倒した敵の数の表示を更新
    /// </summary>
    public void UpdateDisplayKillEnemyCount(int killCount)
    {
        txtKillEnemyCount.text = killCount + "体";
    }
}
