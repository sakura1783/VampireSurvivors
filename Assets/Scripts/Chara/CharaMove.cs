using UnityEngine;

/// <summary>
/// キャラ移動用のクラス
/// </summary>
public class CharaMove : MonoBehaviour, IChara
{
    [SerializeField] private float moveSpeed;

    //[SerializeField] private float limitPosX;
    //[SerializeField] private float limitPosY;
    [SerializeField] private Transform leftBottomLimitTran;
    [SerializeField] private Transform rightTopLimitTran;


    /// <summary>
    /// インターフェースで強制的に実装されるメソッド
    /// </summary>
    public void SetUpChara()
    {
        Debug.Log("CharaMove設定完了");
    }

    /// <summary>
    /// 移動
    /// </summary>
    public void Move(Vector2 tapPos)
    {
        Vector2 newPos = Vector2.MoveTowards(transform.position, tapPos, moveSpeed * Time.deltaTime);

        //マップの範囲外にでないように制限をかける
        //newPos.x = Mathf.Clamp(newPos.x, -limitPosX, limitPosX);
        //newPos.y = Mathf.Clamp(newPos.y, -limitPosY, limitPosY);
        newPos.x = Mathf.Clamp(newPos.x, leftBottomLimitTran.position.x, rightTopLimitTran.position.x);
        newPos.y = Mathf.Clamp(newPos.y, leftBottomLimitTran.position.y, rightTopLimitTran.position.y);

        transform.position = newPos;
    }
}
