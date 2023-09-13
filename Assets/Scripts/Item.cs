﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemPopUp itemPop;

    [SerializeField] private CharaController charaController;

    //アイテム効果は重複しない(効果中に同じアイテムを獲得しても何も起こらない)
    private bool isAttackTimeReduced = false;
    public bool IsAttackTimeReduced => isAttackTimeReduced;

    private bool isInvincible = false;
    public bool IsInvincible => isInvincible;

    private bool isShielded = false;
    public bool IsShielded => isShielded;

    private bool isPoisoned = false;

    private bool hasReviveItem = false;


    /// <summary>
    /// アイテム効果を適用する
    /// </summary>
    public void ApplyItemEffect(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.ヒーリングポーション:
                HealingPortion();
                break;

            case ItemType.アタックポーション:
                AttackPortion(itemType);
                break;

            case ItemType.インヴィンシブルポーション:
                InvinciblePortion(itemType);
                break;

            case ItemType.ガーディアンシールド:
                GuardianShield(itemType);
                break;

            case ItemType.ヴェノムドリンク:
                VenomDrink(itemType);
                break;

            default:
                Debug.Log("どれにも当てはまらない、またはリバイブを手に入れた");
                break;
        }

        Debug.Log($"{itemType}の効果が適用されます");
    }

    /// <summary>
    /// 効果時間の計測
    /// </summary>
    /// <param name="itemType"></param>
    private void ItemEffectTimer(ItemType itemType)
    {
        float timer = 0;

        ItemDataSO.ItemData itemData = itemPop.GetItemDataByItemType(itemType);

        while (timer < itemData.effectDuration)
        {
            timer += Time.deltaTime;
        }

        //効果時間を過ぎたらアイテム効果を無くす
        switch (itemType)
        {
            case ItemType.アタックポーション:
                isAttackTimeReduced = false;
                break;

            case ItemType.インヴィンシブルポーション:
                isInvincible = false;
                break;

            case ItemType.ガーディアンシールド:
                isShielded = false;
                break;

            case ItemType.ヴェノムドリンク:
                isPoisoned = false;
                break;
        }

        Debug.Log($"{itemType}の効果が切れました");
    }

    /// <summary>
    /// ヒーリングポーション
    /// </summary>
    private void HealingPortion()
    {
        charaController.UpdateHp(5);
    }

    /// <summary>
    /// アタックポーション
    /// </summary>
    private void AttackPortion(ItemType itemType)
    {
        isAttackTimeReduced = true;

        ItemEffectTimer(itemType);
    }

    /// <summary>
    /// インヴィンシブルポーション
    /// </summary>
    /// <param name="itemType"></param>
    private void InvinciblePortion(ItemType itemType)
    {
        isInvincible = true;

        ItemEffectTimer(itemType);
    }

    /// <summary>
    /// ガーディアンシールド
    /// </summary>
    /// <param name="itemType"></param>
    private void GuardianShield(ItemType itemType)
    {
        isShielded = true;

        ItemEffectTimer(itemType);
    }

    /// <summary>
    /// リバイブ
    /// </summary>
    public void Revive()  //TODO 死亡時に呼び出す
    {
        //プレイヤーが死亡状態であれば(再確認)
        if (charaController.hp <= 0)
        {
            charaController.hp = charaController.maxHp / 2;  //もし答えが少数だった場合は小数点以下は切り捨てられる

            hasReviveItem = false;  //複数回使えないようにする
        }
    }

    /// <summary>
    /// ヴェノムドリンク
    /// </summary>
    /// <param name="itemType"></param>
    private void VenomDrink(ItemType itemType)
    {
        isPoisoned = true;

        ItemEffectTimer(itemType);

        float timer = 0;

        while (isPoisoned)
        {
            timer += Time.deltaTime;

            if (timer >= 2.4f)  //<= 10秒で体力が必ず4減るように値を少し小さめに設定
            {
                timer = 0;

                charaController.UpdateHp(-1);
            }
        }
    }
}