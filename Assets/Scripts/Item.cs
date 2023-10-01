using System.Collections;
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

    [SerializeField] private bool hasReviveItem = false;
    public bool HasReviveItem => hasReviveItem;


    /// <summary>
    /// アイテム効果を適用する
    /// </summary>
    public void ApplyItemEffect(ItemType itemType)
    {
        //すでに効果を持っている場合は、switch文から抜けて、重ねて効果を発動しないようにする
        switch (itemType)
        {
            case ItemType.アタックポーション:
                if (isAttackTimeReduced)
                {
                    return;
                }
                break;

            case ItemType.インヴィンシブルポーション:
                if (isInvincible)
                {
                    return;
                }
                break;

            case ItemType.ガーディアンシールド:
                if (isShielded)
                {
                    return;
                }
                break;

            case ItemType.ヴェノムドリンク:
                if (isPoisoned)
                {
                    return;
                }
                break;
        }

        //アイテムの効果を発動
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

            case ItemType.リバイブ:
                hasReviveItem = true;
                break;

            case ItemType.ヴェノムドリンク:
                StartCoroutine(VenomDrink(itemType));
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
    private IEnumerator ItemEffectTimer(ItemType itemType)
    {
        float timer = 0;

        ItemDataSO.ItemData itemData = itemPop.GetItemDataByItemType(itemType);

        while (timer < itemData.effectDuration)
        {
            timer += Time.deltaTime;

            yield return null;
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
        //エフェクト
        GameObject effect = Instantiate(EffectManager.instance.GetEffect(EffectName.HealEffect), charaController.transform);

        //SE再生
        AudioManager.instance.PlaySE(SeType.ApplyItemEffect);

        charaController.UpdateHp(5);
    }

    /// <summary>
    /// アタックポーション
    /// </summary>
    private void AttackPortion(ItemType itemType)
    {
        //エフェクト
        GameObject effect = Instantiate(EffectManager.instance.GetEffect(EffectName.GoodItemEffect), charaController.transform);
        Destroy(effect, 2f);  //このエフェクトは永遠に続くので2秒で破壊する

        //SE再生
        AudioManager.instance.PlaySE(SeType.ApplyItemEffect);

        isAttackTimeReduced = true;

        StartCoroutine(ItemEffectTimer(itemType));
    }

    /// <summary>
    /// インヴィンシブルポーション
    /// </summary>
    /// <param name="itemType"></param>
    private void InvinciblePortion(ItemType itemType)
    {
        //エフェクト
        GameObject effect = Instantiate(EffectManager.instance.GetEffect(EffectName.GoodItemEffect), charaController.transform);
        Destroy(effect, 2f);

        //SE再生
        AudioManager.instance.PlaySE(SeType.ApplyItemEffect);

        isInvincible = true;

        StartCoroutine(ItemEffectTimer(itemType));
    }

    /// <summary>
    /// ガーディアンシールド
    /// </summary>
    /// <param name="itemType"></param>
    private void GuardianShield(ItemType itemType)
    {
        //エフェクト
        GameObject effect = Instantiate(EffectManager.instance.GetEffect(EffectName.GoodItemEffect), charaController.transform);
        Destroy(effect, 2f);

        //SE再生
        AudioManager.instance.PlaySE(SeType.ApplyItemEffect);

        isShielded = true;

        StartCoroutine(ItemEffectTimer(itemType));
    }

    /// <summary>
    /// リバイブ
    /// </summary>
    public void Revive()
    {
        //エフェクト
        GameObject effect = Instantiate(EffectManager.instance.GetEffect(EffectName.GoodItemEffect), charaController.transform);
        Destroy(effect, 2f);

        //SE再生
        AudioManager.instance.PlaySE(SeType.ApplyItemEffect);

        //プレイヤーが死亡状態であれば(再確認)
        if (charaController.hp <= 0)
        {
            hasReviveItem = false;  //複数回使えないようにする

            //最大HPの半分で生き返る
            charaController.UpdateHp(charaController.maxHp / 2);  //もし答えが少数だった場合は小数点以下は切り捨てられる
        }
    }

    /// <summary>
    /// ヴェノムドリンク
    /// </summary>
    /// <param name="itemType"></param>
    private IEnumerator VenomDrink(ItemType itemType)
    {
        //エフェクト
        GameObject effect = Instantiate(EffectManager.instance.GetEffect(EffectName.BadItemEffect), charaController.transform);

        //SE再生
        AudioManager.instance.PlaySE(SeType.ApplyItemEffect);

        isPoisoned = true;

        StartCoroutine(ItemEffectTimer(itemType));

        float timer = 0;

        while (isPoisoned)
        {
            timer += Time.deltaTime;

            if (timer >= 2.4f)  //<= 10秒で体力が必ず4減るように値を少し小さめに設定
            {
                timer = 0;

                charaController.UpdateHp(-1);
            }

            yield return null;
        }
    }
}
