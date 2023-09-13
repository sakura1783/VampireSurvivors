using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    //カタカナ、漢字に変更
    ヒーリングポーション,  //HP回復
    アタックポーション,  //攻撃までの時間短縮
    インヴィンシブルポーション,  //無敵
    ガーディアンシールド,  //被ダメージ減
    //ShadowVeil,  //影と一体化
    //EncounterReduct,  //敵の生成を減らす
    //Expinc,  //経験値の増加
    リバイブ,  //死亡時、一度だけ生き返る(復活時のHP決める)

    ヴェノムドリンク,  //毒状態になる
    //SpeedReductionPortion,  //移動スピード減

    //count,  //enumの要素数

    None,
}

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Create ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    public List<ItemData> itemDataList = new();


    [System.Serializable]
    public class ItemData
    {
        public int itemNo;
        //public string itemName;  //itemTypeに変更
        public ItemType itemType;
        //public int maxGenerateCount;
        public float effectDuration;
        //public int generateRate;
        public Sprite itemSprite;
        [Multiline] public string descliption;
    }
}
