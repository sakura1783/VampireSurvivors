using UnityEngine;

public class RotateObjectWithMouse : MonoBehaviour
{
    void Update()
    {
        //マウスの位置をワールド座標に変換
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;  //ゲームオブジェクトのZ座標に合わせる

        //マウスの位置とゲームオブジェクトの位置から回転角度を計算
        Vector3 direction = mousePosition - transform.position;
        float rotateionAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;  //角度に置き換える計算にはMathf.Atan2とMathf.Rad2Degを利用できる。Atan2の引数にdirectionを渡し、Rad2Degでラジアンから度数に変換

        //ゲームオブジェクトを回転
        transform.rotation = Quaternion.Euler(0, 0, rotateionAngle);
    }
}
