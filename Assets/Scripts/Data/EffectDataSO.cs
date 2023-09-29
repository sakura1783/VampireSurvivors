using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectName
{
    Hit,
    //EnemyDead,
    TreasureAppear,  //宝箱出現
    HealEffect,
    GoodItemEffect,
    BadItemEffect,
}

[CreateAssetMenu(menuName = "Create EffectDataSO", fileName = "EffectDataSO")]
public class EffectDataSO : ScriptableObject
{
    public List<EffectData> effectDataList = new();


    [System.Serializable]
    public class EffectData
    {
        public EffectName effectName;
        public GameObject effectPrefab;
    }
}
