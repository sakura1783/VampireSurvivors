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

    [SerializeField] private float attackInterval;

    [SerializeField] private Bullet bulletPrefab;

    [SerializeField] private Transform temporaryGameObjectsPlace;

    private Animator charaAnim;

    private float charaScale;  //キャラの左右アニメの設定で利用する

    private Vector2 direction;  //キャラが向いている方向


    void Start()
    {
        SetUpCharaController();

        charaScale = transform.localScale.x;

        StartCoroutine(PrepareAttack());
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

        //transform.position = newPos;  //この時点でtransform.positionの値はnewPosになっている

        //アニメーション
        direction = (newPos - (Vector2)transform.position).normalized;  //上ですでにtransform.positionの値はnewPosになっているのでDebugを入れて確認できる通り、directionの値は全て0になる。よって思い通りの挙動にならない。

        transform.position = newPos;  //記述する位置を修正。この処理はnewPosを計算などで使わなくなってから書くようにする。そうしないと、意図しない挙動になってしまう(今回だと、アニメーションが同期されない)

        charaAnim.SetFloat("X", direction.x);
        charaAnim.SetFloat("Y", direction.y);

        //左右アニメの切り替え(Scaleを変化させて左右移動にアニメを対応させる)
        Vector2 temp = transform.localScale;  //<= temp(一時的な)変数に現在のlocalScaleの値を代入
        temp.x = direction.x;  //目的地の方向をtemp変数に代入
        if (temp.x < 0)
        {
            //0よりも小さければScaleを1にする
            temp.x = charaScale;
        }
        else
        {
            //0よりも大きければScaleを-1にする
            temp.x = -charaScale;
        }
        transform.localScale = temp;
    }

    /// <summary>
    /// 階段のコライダーと接触した際、isClimbedStairの値を変える
    /// </summary>
    /// <param name="col"></param>
    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col == leftStairTrigger || col == rightStairTrigger)
    //    {
    //        if (!mapManager.isClimbedStairs)
    //        {
    //            mapManager.isClimbedStairs = true;

    //            mapManager.JudgeClimbedStairs();

    //            return;
    //        }

    //        if (mapManager.isClimbedStairs)
    //        {
    //            mapManager.isClimbedStairs = false;

    //            mapManager.JudgeClimbedStairs();

    //            return;
    //        }
    //    }
    //}

    /// <summary>
    /// 攻撃準備
    /// </summary>
    private IEnumerator PrepareAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            Attack();
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform);

        bullet.transform.SetParent(temporaryGameObjectsPlace);

        bullet.Shoot(direction);

        Debug.Log("攻撃");
    }
}
