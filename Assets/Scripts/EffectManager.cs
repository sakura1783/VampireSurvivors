using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    [SerializeField] private EffectDataSO effectDataSO;


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

    public GameObject GetEffect(EffectName effectName)
    {
        return effectDataSO.effectDataList.Find(effectData => effectData.effectName == effectName).effectPrefab;
    }
}
