using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

            //ポップアップを表示
            itemPop.ShowPopUp(this.gameObject);
        }
    }
}
