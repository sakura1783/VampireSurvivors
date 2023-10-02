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
    [SerializeField] private Button btnSelect;
    [SerializeField] private Button btnChoose;

    [SerializeField] private NewWeaponButton newWeaponBtnPrefab;

    [SerializeField] private LevelupWeaponButton levelupWeaponBtnPrefab;

    [SerializeField] private Transform newWeaponsPlace;
    [SerializeField] private Transform levelupWeaponsPlace;

    //step2_levelup
    [SerializeField] private Text txtLevelupDescliption;

    //step2_newWeapon
    [SerializeField] private Text txtBulletDescliption;

    [SerializeField] private Transform shurikenPlace;

    [SerializeField] private GameManager gameManager;

    //各BulletGenerator
    private BulletGenerator bulletGenerator;
    public BulletGenerator BulletGenerator => bulletGenerator;

    private BulletGenerator1 bulletGenerator1;
    public BulletGenerator1 BulletGenerator1 => bulletGenerator1;

    private BulletGenerator2 bulletGenerator2;
    public BulletGenerator2 BulletGenerator2 => bulletGenerator2;

    private BulletGenerator3 bulletGenerator3;
    public BulletGenerator3 BulletGenerator3 => bulletGenerator3;

    private BulletGenerator4 bulletGenerator4;
    public BulletGenerator4 BulletGenerator4 => bulletGenerator4;

    private BulletGenerator5 bulletGenerator5;
    public BulletGenerator5 BulletGenerator5 => bulletGenerator5;

    private BulletDataSO.BulletData selectedBulletData;  //選ばれたバレットのデータ

    private CharaController charaController;

    private List<int> attatchedBulletList = new();  //CharaSetにアタッチ済みのバレットのリスト(NewWeapon用)

    private int[] attatchedBulletGeneratorsArray = new int[6];  //CharaSetにアタッチされているBulletGeneratorを順番に入れる(LevelupWeapon用)

    private List<GameObject> btnsList = new();  //生成した全てのボタンのリスト

    //public bool isDisplayPopUp = false;  //ポップアップ表示中かどうか

    private bool isAllWeaponsMaxLevel = false;  //全ての武器が最大レベルに達しているかどうか。達している場合は、レベルアップしてもポップアップを表示しない


    /// <summary>
    /// 初期設定。CharaControllerのSetUpメソッドで実行する
    /// </summary>
    public void SetUpLevelupPopUp(CharaController charaController, List<BulletDataSO.BulletData> bulletDatasList)
    {
        this.charaController = charaController;

        //BulletGeneratorの情報をセット
        bulletGenerator = charaController.BulletGenerator;

        popUpCanvasGroup.alpha = 0;

        //ポップアップの全てのボタンが押せなく(反応しなく)なる
        popUpCanvasGroup.blocksRaycasts = false;

        step2_LevelupCanvasGroup.blocksRaycasts = false;
        step2_NewWeaponCanvasGroup.blocksRaycasts = false;

        SetUpButtons();

        attatchedBulletList.Add(0);  //デフォルトの弾は最初からついているのでリストに追加
    }

    /// <summary>
    /// ボタンの設定
    /// </summary>
    private void SetUpButtons()
    {
        btnLevelup.onClick.AddListener(OnClickButtonLevelup);
        btnNewWeapon.onClick.AddListener(OnClickButtonNewWeapon);
        btnSelect.onClick.AddListener(OnClickButtonSelect);
        btnChoose.onClick.AddListener(OnClickButtonChoose);
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
    /// btnSelectを押した際の処理
    /// </summary>
    private void OnClickButtonSelect()
    {
        //選んだバレットのBulletGeneratorをCharaSetにアタッチ
        AttatchBulletGenerator();

        //CharaSetにアタッチされているBulletGeneratorたちをCharaControllerの各変数に入れる
        charaController.AssainBulletGenerators();

        attatchedBulletList.Add(selectedBulletData.bulletNo);

        HidePopUp();
    }

    /// <summary>
    /// btnChooseを押した際の処理
    /// </summary>
    private void OnClickButtonChoose()
    {
        //選択したバレットをレベルアップ
        LevelUpBullet();

        HidePopUp();
    }

    /// <summary>
    /// ポップアップの表示。CharaControllerのLevelUpメソッドで実行する
    /// </summary>
    public void ShowPopUp(List<BulletDataSO.BulletData> bulletDatasList)
    {
        //btnNewWeaponの生成
        GenerateBtnNewWeapon(bulletDatasList);

        //btnLevelupの生成
        StartCoroutine(GenerateLevelupWeaponBtn());

        //TODO わからない
        //int count = (int)GenerateLevelupWeaponBtn().Current;

        //TODO 追加できる新しい武器があるか、レベルアップできる武器があるかを判断してボタンの描画と押下反応を制御
        //ToggleButtonVisibility();

        //全ての武器を獲得していて、かつ最大レベルまで達している場合、ポップアップ表示の処理をしない
        if (isAllWeaponsMaxLevel)
        {
            return;
        }

        gameManager.IsDisplayPopUp = true;

        popUpCanvasGroup.alpha = 1;

        popUpCanvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    public void HidePopUp()
    {
        gameManager.IsDisplayPopUp = false;

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

        //ToggleButtonVisibilityメソッドで変更を加えたStep1のボタンの押下反応と描画の初期化
        btnNewWeapon.gameObject.SetActive(true);
        btnNewWeapon.interactable = true;

        btnLevelup.gameObject.SetActive(true);
        btnLevelup.interactable = true;

        //生成した全てのボタンを破壊
        foreach (var button in btnsList)
        {
            Destroy(button);
        }

        //txtDescliptionの初期化
        txtBulletDescliption.text = "";
        txtLevelupDescliption.text = "";
    }

    /// <summary>
    /// 追加できる新しい武器またはレベルアップできる武器があるかどうか判断して、ボタンの押下反応と描画を切り替える
    /// </summary>
    private void ToggleButtonVisibility(int newWeaponBtnCount, int levelUpWeaponBtnCount)
    {
        //全ての武器を獲得していて、かつ最大レベルまで達している場合
        if (newWeaponBtnCount == 0 && levelUpWeaponBtnCount == 0)
        {
            isAllWeaponsMaxLevel = true;

            return;
        }

        //全ての武器を獲得している場合
        if (newWeaponBtnCount == 0)
        {
            //描画をしない
            btnNewWeapon.gameObject.SetActive(false);

            //押下反応しない
            btnNewWeapon.interactable = false;
        }

        //持っている武器が全て最大レベルに達している場合
        if (levelUpWeaponBtnCount == 0)
        {
            btnLevelup.gameObject.SetActive(false);

            btnLevelup.interactable = false;
        }
    }

    /// <summary>
    /// btnNewWeaponの生成
    /// </summary>
    private void GenerateBtnNewWeapon(List<BulletDataSO.BulletData> bulletDatasList)
    {
        //まだ手に入れていないバレットが2個未満になった場合、ループを1回にする(つまり、ループをしない)
        if (attatchedBulletList.Count < 5)
        {
            //とりあえずの初期値
            int randomNo = 0;

            //同じバレットのボタンを生成しないための変数
            int usedNo = 0;

            for (int i = 0; i < 2; i++)
            {
                //attatchedBulletNoListにrandomNoが含まれている、またはrandomNoとusedNoが同じ間ずっと繰り返す
                do
                {
                    randomNo = Random.Range(0, bulletDatasList.Count);

                } while (attatchedBulletList.Contains(randomNo) || randomNo == usedNo);

                usedNo = randomNo;

                NewWeaponButton button = Instantiate(newWeaponBtnPrefab, newWeaponsPlace);

                //生成したボタンのゲームオブジェクトをリストに入れる
                btnsList.Add(button.gameObject);

                button.SetUpBtnNewWeapon(this, bulletDatasList[randomNo]);
            }
        }
        else
        {
            int randomNo = 0;

            do
            {
                randomNo = Random.Range(0, bulletDatasList.Count);

            } while (attatchedBulletList.Contains(randomNo));

            NewWeaponButton button = Instantiate(newWeaponBtnPrefab, newWeaponsPlace);

            btnsList.Add(button.gameObject);

            button.SetUpBtnNewWeapon(this, bulletDatasList[randomNo]);
        }
    }

    /// <summary>
    /// 選択されたバレットの情報をポップアップに表示する
    /// </summary>
    public void SetSelectBulletDetail(BulletDataSO.BulletData bulletData, string btnName)
    {
        selectedBulletData = bulletData;
        Debug.Log($"selectedBulletData : {selectedBulletData.bulletNo}");

        if (btnName == "NewWeapon")
        {
            txtBulletDescliption.text = bulletData.descliptionBullet;
        }

        if (btnName == "LevelupWeapon")
        {
            txtLevelupDescliption.text = bulletData.descliptionLevelup;
        }
    }

    /// <summary>
    /// どのBulletGeneratorにするか決めて、アタッチ
    /// </summary>
    private void AttatchBulletGenerator()
    {
        switch (selectedBulletData.bulletNo)
        {
            case 1:
                bulletGenerator1 = charaController.gameObject.AddComponent<BulletGenerator1>();
                bulletGenerator1.SetUpBulletGenerator1(charaController, shurikenPlace);
                bulletGenerator1.PrepareGenerateBullet(charaController.Direction);
                Debug.Log($"bulletNo：{selectedBulletData.bulletNo}、BulletGenerator1をアタッチします");
                break;

            case 2:
                bulletGenerator2 = charaController.gameObject.AddComponent<BulletGenerator2>();
                bulletGenerator2.SetUpBulletGenerator2(charaController);
                Debug.Log($"bulletNo：{selectedBulletData.bulletNo}、BulletGenerator2をアタッチします");
                break;

            case 3:
                bulletGenerator3 = charaController.gameObject.AddComponent<BulletGenerator3>();
                bulletGenerator3.SetUpBulletGenerator3(charaController);
                Debug.Log($"bulletNo：{selectedBulletData.bulletNo}、BulletGenerator3をアタッチします");
                break;

            case 4:
                bulletGenerator4 = charaController.gameObject.AddComponent<BulletGenerator4>();
                bulletGenerator4.attackInterval = selectedBulletData.attackInterval;  //攻撃のインターバル時間を設定
                bulletGenerator4.SetUpBulletGenerator4(charaController);
                Debug.Log($"bulletNo：{selectedBulletData.bulletNo}、BulletGenerator4をアタッチします");
                break;

            case 5:
                bulletGenerator5 = charaController.gameObject.AddComponent<BulletGenerator5>();
                bulletGenerator5.SetUpBulletGenerator5(charaController);
                Debug.Log($"bulletNo：{selectedBulletData.bulletNo}、BulletGenerator5をアタッチします");
                break;
        }
    }

    /// <summary>
    /// btnLevelupWeaponの生成
    /// </summary>
    private IEnumerator GenerateLevelupWeaponBtn()
    {
        //TODO いくつボタンが生成されたか
        //int count = 0;

        //CharaSetにアタッチされているBulletGeneratorの番号を配列に順番に入れる
        yield return StartCoroutine(CreateAttatchedBulletGeneratorsArray());

        //foreach (var count in attatchedBulletGeneratorsArray) //この場合、countとはattactchedBulletGenratorsArrayの各要素の中身の番号を表すので(例えばこのクラスの場合、1個目にはは10が入っている)、これだとIndexOutOfRangeExceptionエラーが出てしまう
        //上の処理を修正。for文に変更
        for (int i = 0; i < attatchedBulletGeneratorsArray.Length; i++)
        {
            //配列の中身が初期値の0(つまり、BulletGeneratorがCharaSetにアタッチされていない)の場合、スキップ(ボタンを生成しない)
            if (attatchedBulletGeneratorsArray[i] == 0)
            {
                continue;
            }

            //bulletLevelがmaxLevelに達している場合、スキップ
            if (ReachedMaxLevel(i))
            {
                continue;
            }

            LevelupWeaponButton button = Instantiate(levelupWeaponBtnPrefab, levelupWeaponsPlace);

            //生成したボタンのゲームオブジェクトをリストに追加
            btnsList.Add(button.gameObject);

            //ボタンの設定
            button.SetUpLevelupWeaponBtn(this, charaController.bulletDatasList[i]);

            //count++;
        }

        //yield return count;
    }

    /// <summary>
    /// CharaSetにアタッチされているBulletGeneratorの番号を配列に順番に入れる
    /// </summary>
    private IEnumerator CreateAttatchedBulletGeneratorsArray()
    {
        attatchedBulletGeneratorsArray[0] = 10;  //適当な大きな数字を代入

        if (charaController.BulletGenerator1)
        {
            attatchedBulletGeneratorsArray[1] = 1;
        }
        if (charaController.BulletGenerator2)
        {
            attatchedBulletGeneratorsArray[2] = 2;
        }
        if (charaController.BulletGenerator3)
        {
            attatchedBulletGeneratorsArray[3] = 3;
        }
        if (charaController.BulletGenerator4)
        {
            attatchedBulletGeneratorsArray[4] = 4;
        }
        if (charaController.BulletGenerator5)
        {
            attatchedBulletGeneratorsArray[5] = 5;
        }

        yield return null;
    }

    /// <summary>
    /// bulletLevelがmaxLevelに達しているか判断する
    /// </summary>
    private bool ReachedMaxLevel(int count)
    {
        switch (count)
        {
            case 0:
                if (bulletGenerator.bulletLevel >= charaController.bulletDatasList[count].maxLevel)
                {
                    return true;
                }
                return false;

            case 1:
                if (bulletGenerator1.bulletLevel >= charaController.bulletDatasList[count].maxLevel)
                {
                    return true;
                }
                return false;

            case 2:
                if (bulletGenerator2.bulletLevel >= charaController.bulletDatasList[count].maxLevel)
                {
                    return true;
                }
                return false;

            case 3:
                if (bulletGenerator3.bulletLevel >= charaController.bulletDatasList[count].maxLevel)
                {
                    return true;
                }
                return false;

            case 4:
                if (bulletGenerator4.bulletLevel >= charaController.bulletDatasList[count].maxLevel)
                {
                    return true;
                }
                return false;

            case 5:
                if (bulletGenerator5.bulletLevel >= charaController.bulletDatasList[count].maxLevel)
                {
                    return true;
                }
                return false;

            default:
                return false;
        }
    }

    /// <summary>
    /// 選ばれたバレットをレベルアップ
    /// </summary>
    private void LevelUpBullet()
    {
        switch (selectedBulletData.bulletNo)
        {
            case 0:
                bulletGenerator.bulletLevel++;
                break;

            case 1:
                bulletGenerator1.bulletLevel++;
                bulletGenerator1.PrepareGenerateBullet(charaController.Direction);
                break;

            case 2:
                bulletGenerator2.bulletLevel++;
                break;

            case 3:
                bulletGenerator3.bulletLevel++;
                break;

            case 4:
                bulletGenerator4.bulletLevel++;
                bulletGenerator4.SetAttackIntervalByLevel();
                break;

            case 5:
                bulletGenerator5.bulletLevel++;
                break;
        }
    }
}
