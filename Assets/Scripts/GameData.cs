using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public List<EnemyController> enemiesList = new();

    public List<EnemyController> targetList = new();  //追尾弾の追尾対象のリスト(追尾対象が被らないようにする)


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
