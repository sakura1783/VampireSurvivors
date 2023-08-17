using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewWeaponButton : MonoBehaviour
{
    [SerializeField] private Image imgNewWeaponBtn;

    [SerializeField] private Button btnNewWeapon;

    private BulletDataSO.BulletData bulletData;

    private LevelupPopUp levelupPop;


    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="bulletData"></param>
    public void SetUpBtnNewWeapon(LevelupPopUp levelupPop, BulletDataSO.BulletData bulletData)
    {
        this.levelupPop = levelupPop;
        this.bulletData = bulletData;

        imgNewWeaponBtn.sprite = bulletData.bulletSprite;

        btnNewWeapon.onClick.AddListener(OnClickBtnNewWeapon);
    }

    /// <summary>
    /// btnNewWeaponを押した際の処理
    /// </summary>
    private void OnClickBtnNewWeapon()
    {
        levelupPop.SetSelectBulletDetail(bulletData, "NewWeapon");
    }
}
