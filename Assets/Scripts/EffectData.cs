using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectName
{
    enemyHit,
    enemyDown,
    treasure,
}

[System.Serializable]
public class EffectData
{
    public EffectName effectName;
    public GameObject effectPrefab;
}
