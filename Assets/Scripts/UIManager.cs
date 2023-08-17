using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CharaController charaController;

    [SerializeField] private Text txtCharaLevel;
    
    /// <summary>
    /// キャラのレベルのUIを更新
    /// </summary>
    public void UpdateDisplayCharaLevel()
    {
        txtCharaLevel.text = "Lv." + charaController.charaLevel;
    }
}
