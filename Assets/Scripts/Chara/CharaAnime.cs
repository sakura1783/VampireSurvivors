using UnityEngine;

/// <summary>
/// キャラのアニメの同期クラス
/// </summary>
public class CharaAnime : MonoBehaviour, IChara
{
    private Animator charaAnim;

    private float charaScale;  //キャラの左右アニメの設定で利用する


    /// <summary>
    /// インターフェースで強制的に実装されるメソッド
    /// </summary>
    public void SetUpChara()
    {
        transform.GetChild(0).gameObject.TryGetComponent(out charaAnim);

        charaScale = transform.localScale.x;  //キャラの向き変更に使う

        Debug.Log("CharaAnime設定完了");
    }

    public void UpdateAnimation(Vector2 direction)
    {
        charaAnim.SetFloat("X", direction.x);
        charaAnim.SetFloat("Y", direction.y);

        //左右アニメの切り替え(Scaleを変化させて左右移動にアニメを対応させる)
        Vector2 temp = transform.localScale;  //<= temp(一時的な)変数に現在のlocalScaleの値を代入

        //temp.x = direction.x;  //目的地の方向をtemp変数に代入
        //if (temp.x < 0)
        //{
        //    //0よりも小さければScaleを1にする
        //    temp.x = charaScale;
        //}
        //else
        //{
        //    //0よりも大きければScaleを-1にする
        //    temp.x = -charaScale;
        //}

        //三項演算子に置き換えた場合(if/else文で、同じ変数に代入処理する時に使える)
        temp.x = direction.x < 0 ? charaScale : -charaScale;

        transform.localScale = temp;
    }
}
