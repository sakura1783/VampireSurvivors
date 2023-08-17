using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelupWeaponButton : MonoBehaviour
{
    [SerializeField] private Image imgLevelupWeaponBtn;

    [SerializeField] private Button btnLevelupWeapon;

    [SerializeField] private Text txtLevel;

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

        DisplayTxtLevel();

        btnLevelupWeapon.onClick.AddListener(OnClickBtnLevelupWeapon);
    }

    /// <summary>
    /// 現在のレベルと次のレベルをTextに表示
    /// </summary>
    private void DisplayTxtLevel()
    {
        switch (bulletData.bulletNo)
        {
            case 0:
                int bulletLevel0 = levelupPop.BulletGenerator.bulletLevel;
                txtLevel.text = bulletLevel0 + " → " + (bulletLevel0 += 1);
                break;

            case 1:
                int bulletLevel1 = levelupPop.BulletGenerator1.bulletLevel;
                txtLevel.text = bulletLevel1 + " → " + (bulletLevel1 += 1);
                break;

            case 2:
                int bulletLevel2 = levelupPop.BulletGenerator2.bulletLevel;
                txtLevel.text = bulletLevel2 + " → " + (bulletLevel2 += 1);
                break;

            //case 3:
            //    int bulletLevel3 = levelupPop.BulletGenerator3.bulletLevel;
            //    txtLevel.text = bulletLevel3 + " → " + (bulletLevel3 += 1);
            //    break;

            case 4:
                int bulletLevel4 = levelupPop.BulletGenerator4.bulletLevel;
                txtLevel.text = bulletLevel4 + " → " + (bulletLevel4 += 1);
                break;

            case 5:
                int bulletLevel5 = levelupPop.BulletGenerator5.bulletLevel;
                txtLevel.text = bulletLevel5 + " → " + (bulletLevel5 += 1);
                break;
        }
    }

    /// <summary>
    /// btnLevelupWeaponを押した際の処理
    /// </summary>
    private void OnClickBtnLevelupWeapon()
    {
        levelupPop.SetSelectBulletDetail(bulletData, "LevelupWeapon");
    }
}
