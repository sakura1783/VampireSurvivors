using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelupWeaponButton : MonoBehaviour
{
    [SerializeField] private Image imgLevelupWeaponBtn;

    [SerializeField] private Button btnLevelupWeapon;

    private LevelupPopUp levelupPop;

    private BulletDataSO.BulletData bulletData;


    /// <summary>
    /// 設定
    /// </summary>
    public void SetUpLevelupWeaponBtn(LevelupPopUp levelupPop, BulletDataSO.BulletData bulletData)
    {
        this.levelupPop = levelupPop;
        this.bulletData = bulletData;

        imgLevelupWeaponBtn.sprite = bulletData.bulletSprite;

        btnLevelupWeapon.onClick.AddListener(OnClickBtnLevelupWeapon);
    }

    /// <summary>
    /// btnLevelupWeaponを押した際の処理
    /// </summary>
    private void OnClickBtnLevelupWeapon()
    {
        levelupPop.SetSelectBulletDetail(bulletData, "LevelupWeapon");
    }
}
