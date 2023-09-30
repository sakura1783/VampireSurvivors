using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TreasureChest : MonoBehaviour
{
    private ItemPopUp itemPop;


    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpTreasureChest(ItemPopUp itemPop)
    {
        this.itemPop = itemPop;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //ランダムにアイテムを決定し、ポップアップに情報を渡す
            //GetRandomItem();

            //SE再生(timeScaleが0になってもAudioSourceを使用して再生されている音は影響されない)
            AudioManager.instance.PlaySE(SeType.OpenTreasureChest);

            //ポップアップを表示
            itemPop.ShowPopUp(this.gameObject);
        }
    }
}
