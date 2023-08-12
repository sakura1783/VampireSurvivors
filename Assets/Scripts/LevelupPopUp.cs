using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelupPopUp : MonoBehaviour
{
    [SerializeField] private CanvasGroup popUpCanvasGroup;
    [SerializeField] private CanvasGroup step1CanvasGroup;
    [SerializeField] private CanvasGroup step2_LevelupCanvasGroup;
    [SerializeField] private CanvasGroup step2_NewWeaponCanvasGroup;

    [SerializeField] private Button btnLevelup;
    [SerializeField] private Button btnNewWeapon;


    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpLevelupPopUp()
    {
        popUpCanvasGroup.alpha = 0;

        //ポップアップの全てのボタンが押せなく(反応しなく)なる
        popUpCanvasGroup.blocksRaycasts = false;

        step2_LevelupCanvasGroup.blocksRaycasts = false;
        step2_NewWeaponCanvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// ボタンの設定
    /// </summary>
    private void SetUpButtons()
    {
        btnLevelup.onClick.AddListener(OnClickButtonLevelup);
        btnNewWeapon.onClick.AddListener(OnClickButtonNewWeapon);
    }

    /// <summary>
    /// btnLevelupを押した際の処理
    /// </summary>
    private void OnClickButtonLevelup()
    {
        step1CanvasGroup.alpha = 0;
        step1CanvasGroup.blocksRaycasts = false;

        step2_LevelupCanvasGroup.alpha = 1;
        step2_LevelupCanvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// btnNewWeaponを押した際の処理
    /// </summary>
    private void OnClickButtonNewWeapon()
    {
        step1CanvasGroup.alpha = 0;
        step1CanvasGroup.blocksRaycasts = false;

        step2_NewWeaponCanvasGroup.alpha = 1;
        step2_NewWeaponCanvasGroup.blocksRaycasts = true;
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
        popUpCanvasGroup.alpha = 0;

        popUpCanvasGroup.blocksRaycasts = false;

        InitializePopUp();
    }

    /// <summary>
    /// ボタンのアクティブ状態、alpha値などを初期値に戻す(2回目以降の表示のため)
    /// </summary>
    private void InitializePopUp()
    {
        //alpha値
        popUpCanvasGroup.alpha = 0;

        step1CanvasGroup.alpha = 1;
        step2_LevelupCanvasGroup.alpha = 0;
        step2_NewWeaponCanvasGroup.alpha = 0;

        //ボタンのアクティブ状態
        popUpCanvasGroup.blocksRaycasts = false;

        step1CanvasGroup.blocksRaycasts = true;
        step2_LevelupCanvasGroup.blocksRaycasts = false;
        step2_NewWeaponCanvasGroup.blocksRaycasts = false;
    }
}
