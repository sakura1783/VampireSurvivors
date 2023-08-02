using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    [SerializeField] private float limitPosX;
    [SerializeField] private float limitPosY;

    [SerializeField] private MapManager mapManager;

    [SerializeField] private Collider2D leftStairTrigger;
    [SerializeField] private Collider2D rightStairTrigger;  //isClimbedStairを判定するための階段のコライダー

    private Animator charaAnim;

    private float charaScale;  //キャラの左右アニメの設定で利用する


    void Start()
    {
        SetUpCharaController();

        charaScale = transform.localScale.x;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 tapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Move(tapPos);
        }
    }

    /// <summary>
    /// 設定
    /// </summary>
    public void SetUpCharaController()
    {
        transform.GetChild(0).gameObject.TryGetComponent(out charaAnim);
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move(Vector2 tapPos)
    {
        Vector2 newPos = Vector2.MoveTowards(transform.position, tapPos, moveSpeed * Time.deltaTime);

        //マップの範囲外にでないように制限をかける
        newPos.x = Mathf.Clamp(newPos.x, -limitPosX, limitPosX);
        newPos.y = Mathf.Clamp(newPos.y, -limitPosY, limitPosY);

        transform.position = newPos;

        //アニメーション
        Vector2 direction = (newPos - (Vector2)transform.position).normalized;

        charaAnim.SetFloat("X", direction.x);
        charaAnim.SetFloat("Y", direction.y);

        //左右アニメの切り替え(Scaleを変化させて左右移動にアニメを対応させる)
        Vector2 temp = transform.localScale;  //<= temp(一時的な)変数に現在のlocalScaleの値を代入
        temp.x = direction.x;  //目的地の方向をtemp変数に代入
        if (temp.x > 0)
        {
            //0よりも大きければScaleを1にする
            temp.x = charaScale;
        }
        else
        {
            //0よりも小さければScaleを-1にする
            temp.x = -charaScale;
        }
        transform.localScale = temp;
    }

    /// <summary>
    /// 階段のコライダーと接触した際、isClimbedStairの値を変える
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col == leftStairTrigger || col == rightStairTrigger)
        {
            if (!mapManager.isClimbedStairs)
            {
                mapManager.isClimbedStairs = true;

                return;
            }

            if (mapManager.isClimbedStairs)
            {
                mapManager.isClimbedStairs = false;

                return;
            }
        }

        //isClimbedStairsに応じて、コライダーのオンオフを切り替える
        mapManager.JudgeClimbedStairs();
    }
}
