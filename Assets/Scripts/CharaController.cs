using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaController : MonoBehaviour
{
    public int level = 1;

    [SerializeField] private float moveSpeed;

    //[SerializeField] private float limitPosX;
    //[SerializeField] private float limitPosY;
    [SerializeField] private Transform leftBottomLimitTran;
    [SerializeField] private Transform rightTopLimitTran;

    //[SerializeField] private MapManager mapManager;

    //[SerializeField] private Collider2D leftStairTrigger;
    //[SerializeField] private Collider2D rightStairTrigger;  //isClimbedStairを判定するための階段のコライダー

    [SerializeField] private float attackInterval;

    //[SerializeField] private Bullet bulletPrefab;

    //[SerializeField] private Transform temporaryObjectsPlace;

    [SerializeField] private int hp;

    [SerializeField] private int needExpForLevelUp;  //レベルアップに必要なExp

    [SerializeField] private int totalExp;  //現在保持しているExp

    [SerializeField] private BulletGenerator bulletGenerator;

    [SerializeField] private EnemyGenerator enemyGenerator1;
    [SerializeField] private EnemyGenerator enemyGenerator2;

    private Animator charaAnim;

    private float charaScale;  //キャラの左右アニメの設定で利用する

    private Vector2 direction;  //キャラが向いている方向

    private int addPoint = 5;  //needExpForLevelUp変数に加算するポイント(レベルが上がるにつれて、必要なExpも増える)


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

        charaScale = transform.localScale.x;  //キャラの向き変更に使う

        StartCoroutine(PrepareAttack());
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move(Vector2 tapPos)
    {
        Vector2 newPos = Vector2.MoveTowards(transform.position, tapPos, moveSpeed * Time.deltaTime);

        //マップの範囲外にでないように制限をかける
        //newPos.x = Mathf.Clamp(newPos.x, -limitPosX, limitPosX);
        //newPos.y = Mathf.Clamp(newPos.y, -limitPosY, limitPosY);
        newPos.x = Mathf.Clamp(newPos.x, leftBottomLimitTran.position.x, rightTopLimitTran.position.x);
        newPos.y = Mathf.Clamp(newPos.y, leftBottomLimitTran.position.y, rightTopLimitTran.position.y);

        //transform.position = newPos;  //この時点でtransform.positionの値はnewPosになっている。よってここに書くと思い通りの挙動にならないのでコメントアウト

        //アニメーション
        direction = (newPos - (Vector2)transform.position).normalized;  //上ですでにtransform.positionの値はnewPosになっているのでDebugを入れて確認できる通り、directionの値は全て0になる。よって思い通りの挙動にならない。

        transform.position = newPos;  //記述する位置を修正。この処理はnewPosを計算などで使わなくなってから書くようにする。そうしないと、意図しない挙動になってしまう(今回だと、アニメーションが同期されない)

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

        //上の処理を三項演算子で記述
        temp.x = direction.x < 0 ? charaScale : -charaScale;

        transform.localScale = temp;
    }

    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    ////階段のコライダーと接触した際、isClimbedStairの値を変え、コライダーのアクティブ状態を切り替える
    //    //if (col == leftStairTrigger || col == rightStairTrigger)
    //    //{
    //    //    if (!mapManager.isClimbedStairs)
    //    //    {
    //    //        mapManager.isClimbedStairs = true;

    //    //        mapManager.JudgeClimbedStairs();

    //    //        return;
    //    //    }

    //    //    if (mapManager.isClimbedStairs)
    //    //    {
    //    //        mapManager.isClimbedStairs = false;

    //    //        mapManager.JudgeClimbedStairs();

    //    //        return;
    //    //    }
    //    //}

    //    if (col.CompareTag("Enemy"))
    //    {
    //        Destroy(col.gameObject);

    //        UpdateHp(-col.GetComponent<EnemyController>().attackPoint);
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
        //Bullet bullet = Instantiate(bulletPrefab, transform);
        //bullet.transform.SetParent(temporaryObjectsPlace);
        //bullet.Shoot(direction);

        //上の処理をまとめる
        bulletGenerator.PrepareGenerateBullet(direction);

        Debug.Log("攻撃");
    }

    /// <summary>
    /// HP更新
    /// </summary>
    public void UpdateHp(int value)
    {
        hp += value;
    }

    /// <summary>
    /// Expを加算する
    /// </summary>
    /// <param name="exp"></param>
    public void AddExp(int exp)
    {
        totalExp += exp;

        JudgeIsReadyToLevelUp();
    }

    /// <summary>
    /// レベルアップできるかどうかを判断し、それに伴った処理を実行する
    /// </summary>
    private void JudgeIsReadyToLevelUp()
    {
        if (totalExp >= needExpForLevelUp)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// レベルアップ処理
    /// </summary>
    private void LevelUp()
    {
        //Expの値をリセットする
        totalExp = 0;

        level++;

        Debug.Log($"現在のレベル：{level}");

        //次のレベルアップに必要なExpを増やす(レベルが上がるにつれて、必要なExpも増える)
        needExpForLevelUp += addPoint * (level - 1);

        if (level == 5)
        {
            StartCoroutine(enemyGenerator1.GenerateEnemy(this));
        }
        if (level == 10)
        {
            StartCoroutine(enemyGenerator2.GenerateEnemy(this));
        }
    }
}
