using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public List<EnemyController> enemiesList = new();

    public List<EnemyController> targetList = new();  //追尾弾の追尾対象のリスト(追尾対象が被らないようにする)

    //順位ごとのデータ
    public int firstScore;
    public string firstName;

    public int secondScore;
    public string secondName;

    public int thirdScore;
    public string thirdName;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
