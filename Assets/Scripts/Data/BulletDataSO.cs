using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BulletGeneratorの種類
/// </summary>
public enum GeneratorType
{
    RefactorBullet0Generator,
    RefactorBullet1Generator,
    RefactorBullet2Generator,
    RefactorBullet3Generator,
    RefactorBullet4Generator,
    RefactorBullet5Generator,
}

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
        public float destroyTime;
        public int maxLevel;

        public Sprite bulletSprite;

        [Multiline] public string descliptionBullet;
        [Multiline] public string descliptionLevelup;

        public BulletBase bulletPrefab;
        public GeneratorType generatorType;
    }
}
