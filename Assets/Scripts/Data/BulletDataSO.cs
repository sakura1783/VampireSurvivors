using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletDataSO", menuName = "Create BulletDataSO")]
public class BulletDataSO : ScriptableObject
{
    public List<BulletData> bulletDataList = new();


    /// <summary>
    /// バレットの詳細データ
    /// </summary>
    [System.Serializable]
    public class BulletData
    {
        public int bulletNo;
        public float attackInterval;
        public int maxLevel;

        public Sprite bulletSprite;

        [Multiline] public string descliptionBullet;
        [Multiline] public string descliptionLevelup;
    }
}
