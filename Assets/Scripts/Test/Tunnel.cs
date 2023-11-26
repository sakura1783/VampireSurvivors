using UnityEngine;

/// <summary>
/// 描画の優先順位変更トリガー用のオブジェクトにアタッチするスクリプト
/// トリガー用のオブジェクトと画像のゲームオブジェクトは別々で考える
/// </summary>
public class Tunnel : MonoBehaviour
{
    [SerializeField] private Renderer tunnelSprite;  //キャラや敵よりも描画の優先順位の高い画像。トンネル用の画像だが、どのゲームオブジェクトのRendererでもいい。

    public int TunnelSpriteOrderNum => tunnelSprite.sortingOrder;  //トンネル用の画像のOrder in Layerの値のプロパティ
}
