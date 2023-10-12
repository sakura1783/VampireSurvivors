using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;

    public BulletDataSO bulletDataSO;
    public ItemDataSO itemDataSO;
    public GenerateDataSO generateDataSO;


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

    /// <summary>
    /// 弾のデータ取得用
    /// </summary>
    /// <param name="searchNo"></param>
    /// <returns></returns>
    public BulletDataSO.BulletData GetBulletData(int searchNo)
    {
        return bulletDataSO.bulletDataList.Find(data => data.bulletNo == searchNo);
    }
}
