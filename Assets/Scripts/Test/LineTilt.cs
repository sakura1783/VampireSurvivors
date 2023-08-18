using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTilt : MonoBehaviour
{
    public Transform centerPoint;  //中心点の位置

    public CharaController charaController;


    void Update()
    {
        ////マウスの位置をワールド座標に変換
        //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePosition.z = transform.position.z;  //ゲームオブジェクトのZ座標に合わせる

        ////マウスの位置とゲームオブジェクトの位置から回転角度を計算
        //Vector3 direction = mousePosition - transform.position;
        //float rotateionAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;  //角度に置き換える計算にはMathf.Atan2とMathf.Rad2Degを利用できる。Atan2の引数にdirectionを渡し、Rad2Degでラジアンから度数に変換

        ////ゲームオブジェクトを回転
        //transform.rotation = Quaternion.Euler(0, 0, rotateionAngle);

        Vector3 direction = charaController.Direction;

        //Directionの方向を元に傾ける角度を求める
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;  //右端が0で始まるので、水平にするには90足す

        //Quaternion.Eulerで傾けた角度を作成
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        //中心点を基準に直線を傾ける
        Vector3 lineDirection = rotation * Vector3.right;

        //オブジェクトを傾ける
        transform.rotation = rotation;

        //傾けた直線を描画などで使用する
        //Debug.DrawRay(centerPoint.position, lineDirection, Color.red);
    }
}
