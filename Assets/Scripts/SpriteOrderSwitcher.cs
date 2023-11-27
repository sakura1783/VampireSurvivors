using UnityEngine;

/// <summary>
/// プレイヤーの描画処理とマップのコライダー処理
/// </summary>
public class SpriteOrderSwitcher : MonoBehaviour
{
    private SpriteRenderer sr;  //アタッチしたキャラのSpriteRenderer

    private int defaultOrderNum;  //SpriteRendererの初期値。優先順位をもとの状態に戻すときに使う

    private float xCoordinate;  //OnTriggerEnter時にx座標を記録しておく
    private float yCoordinate;

    private MapManager mapManager;

    [SerializeField] private int TunnnelSpriteOrderNum;  //トンネルの描画値


    void Start()
    {
        Reset();

        //mapManager変数に情報を代入する
        //if (gameObject.TryGetComponent(out EnemyController enemyController))
        //{
        //    mapManager = enemyController.GameManager.MapManager;
        //}
        if (gameObject.TryGetComponent(out CharaController charaController))
        {
            mapManager = charaController.GameManager.MapManager;
        }
    }

    private void Reset()
    {
        if (transform.GetChild(0).TryGetComponent(out sr))
        {
            defaultOrderNum = sr.sortingOrder;

            //Debug.Log($"SpriteRendererを取得しました：{sr}");
        }
    }

    //private void OnTriggerStay2D(Collider2D col)
    //{
    //    //キャラがトンネルに侵入している間、キャラの表示優先順位をトンネルよりも低い値に設定する
    //    if (col.TryGetComponent(out Tunnel tunnel))
    //    {
    //        sr.sortingOrder = tunnel.TunnelSpriteOrderNum - 2;  //<= 固定値で値をセットせず変数で指定することで、トンネルにアサインされている画像に対応できる。Wall_Switchよりも小さく設定したいため、ここでは-2を引いている
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D col)
    //{
    //    //キャラがトンネルから出た時、キャラの表示優先順位をもとの値に戻す。
    //    if (col.TryGetComponent(out Tunnel _))  //<= 変数を_にすると、Tunnelクラスを取得はする(判定はする)が、取得した情報はif文内では使わないことを明示的に表現できる
    //    {
    //        sr.sortingOrder = defaultOrderNum;
    //    }
    //}

    /// <summary>
    /// コライダーに入った時、xまたはy座標を記録する
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "LayerTrigger0")  //<= 階段は2つあり、それぞれ向きが違うのでゲームオブジェクトの名前で分岐
        {
            yCoordinate = transform.position.y;

            Debug.Log($"yCoordinate：{yCoordinate}");
        }

        if (other.gameObject.name == "LayerTrigger1")
        {
            xCoordinate = transform.position.x;
        }
    }

    /// <summary>
    /// コライダーに出入りする際の座標の差に応じてコライダーと描画を切り替える
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "LayerTrigger0")
        {
            //コライダーから出た時と入った時の座標が一定以上離れていたら
            if (Mathf.Abs(yCoordinate - transform.position.y) > 2f)
            {
                //if (CompareTag("Player"))
                //{
                //真偽値を反転させる
                mapManager.isClimbedStairs = !mapManager.isClimbedStairs;

                mapManager.JudgeClimbedStairs();
                //}

                //キャラの描画順をトンネルよりも高くする
                SwitchSpriteOrder(mapManager.isClimbedStairs);
            }
            //座標の差があまりない場合、いる位置に変わりはないので何も処理を行わない。
            //else
            //{
            //if (CompareTag("Player"))
            //{
            //    mapManager.isClimbedStairs = false;

            //    mapManager.JudgeClimbedStairs();
            //}

            //キャラの描画順をトンネルよりも低くする(描画順をもとに戻す)
            //SwitchSpriteOrder(mapManager);
            //}
        }

        if (col.gameObject.name == "LayerTrigger1")
        {
            if (Mathf.Abs(xCoordinate - transform.position.x) > 2f)
            {
                //if (CompareTag("Player"))
                //{
                mapManager.isClimbedStairs = !mapManager.isClimbedStairs;

                mapManager.JudgeClimbedStairs();
                //}

                SwitchSpriteOrder(mapManager.isClimbedStairs);
            }
            //else
            //{
            //    if (CompareTag("Player"))
            //    {
            //        mapManager.isClimbedStairs = false;

            //        mapManager.JudgeClimbedStairs();
            //    }

            //    SwitchSpriteOrder(false);
            //}
        }

        //次回の判定に備えて、各変数を初期化
        xCoordinate = 0;
        yCoordinate = 0;
    }

    /// <summary>
    /// 各キャラの描画順の切り替え
    /// </summary>
    /// <param name="bringToFront">キャラの描画をbase_switchよりも前面に持ってくるかどうか</param>
    private void SwitchSpriteOrder(bool bringToFront)
    {
        if (bringToFront)
        {
            sr.sortingOrder = TunnnelSpriteOrderNum + 2;
        }
        else
        {
            sr.sortingOrder = defaultOrderNum;
        }

        //Debug.Log("SwitchSpriteOrderが動きました");
    }
}
