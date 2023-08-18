using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CharaController charaController;

    [SerializeField] private Text txtCharaLevel;
    [SerializeField] private Text txtHp;

    [SerializeField] private Slider slider;
    
    /// <summary>
    /// キャラのレベルのUIを更新
    /// </summary>
    public void UpdateDisplayCharaLevel()
    {
        txtCharaLevel.text = "Lv." + charaController.charaLevel;
    }

    /// <summary>
    /// HPゲージの更新
    /// </summary>
    public void UpdateHpGauge()
    {
        slider.DOValue((float)charaController.hp / (float)charaController.maxHp, 0.5f).SetLink(charaController.gameObject);  //float型にキャストしないとゲージが0になるので注意

        txtHp.text = charaController.hp + "/" + charaController.maxHp;
    }
}
