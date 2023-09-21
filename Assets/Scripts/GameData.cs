using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public List<EnemyController> enemiesList = new();

    public List<EnemyController> targetList = new();  //追尾弾の追尾対象のリスト(追尾対象が被らないようにする)

    //順位ごとのデータ
    //public int firstScore;
    //public string firstName;
    //public int secondScore;
    //public string secondName;
    public int thirdScore;
    //public string thirdName;

    public Dictionary<string, int> playersData = new();  //プレイヤーの名前とスコアのデータ

    public Text txt1stName;
    public Text txt1stScore;

    public Text txt2ndName;
    public Text txt2ndScore;

    public Text txt3rdName;
    public Text txt3rdScore;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            //playersDataディクショナリの初期化
            //playersData = new Dictionary<string, int>() { { "", 0 }, { "", 0 }, { "", 0 } };  //各Keyは一意でなければならない。これだとエラーになる
            playersData = new Dictionary<string, int>() { { "", 0 }, { "-", 0 }, { "--", 0 } };
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
