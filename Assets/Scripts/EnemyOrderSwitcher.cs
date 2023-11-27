using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の描画処理を切り替える
/// </summary>
public class EnemyOrderSwitcher : MonoBehaviour
{
    private SpriteRenderer sr;

    private int defaultOrderNum;

    [SerializeField] private int tunnelOrderNum;


    void Start()
    {
        Reset();
    }

    void Reset()
    {
        if (transform.GetChild(0).TryGetComponent(out sr))
        {
            defaultOrderNum = sr.sortingOrder;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "TunnelTrigger")
        {
            //トンネル上にいる時、描画順位を高くする
            SwitchEnemyOrder(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "TunnelTrigger")
        {
            //トンネル外に出たら、描画順をもとに戻す
            SwitchEnemyOrder(false);
        }
    }

    /// <summary>
    /// 描画順を切り替える
    /// </summary>
    private void SwitchEnemyOrder(bool bringToFront)
    {
        if (bringToFront)
        {
            sr.sortingOrder = tunnelOrderNum + 1;
        }
        else
        {
            sr.sortingOrder = defaultOrderNum;
        }
    }
}
