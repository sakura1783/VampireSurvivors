using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;

    [SerializeField] private Transform temporaryObjectsPlace;

    [SerializeField] private GameManager gameManager;  //CharaController取得用

    [SerializeField] private float offsetDegrees;  //角度の基準値(各バレット同士の角度間隔)


    /// <summary>
    /// バレット生成の準備。レベルに応じてバレットの数を増やす
    /// </summary>
    /// <param name="direction"></param>
    public void PrepareGenerateBullet(Vector2 direction)
    {
        switch (gameManager.CharaController.level)
        {
            case 1:
                GenerateBullet(direction);
                break;

            case 2:
                for (int i = -1; i < 2; i += 2)
                {
                    ////角度の補正値の決定
                    //float offsetAngle = i * offsetDegrees;

                    ////上の補正値を回転させた回転情報を作る(Quaternion.Euler(a, b, c);で、x、y、z軸周りにそれぞれa、b、c度回転する)
                    //Quaternion offsetRotation = Quaternion.Euler(0, 0, offsetAngle);

                    ////上記の回転情報に、directionを掛けることで、ここで弾の方向のベクトルが決まる
                    //Vector2 offsetDirection = offsetRotation * direction;

                    //GenerateBullet(offsetDirection);

                    //上の処理をまとめる
                    GenerateBullet(CalculateBulletDirection(i, direction));
                }
                break;

            case 3:
                for (int i = -1; i < 2; i++)
                {
                    GenerateBullet(CalculateBulletDirection(i, direction));
                }
                break;

            case 4:
                for (int i = -3; i < 4; i += 2)
                {
                    GenerateBullet(CalculateBulletDirection(i, direction));
                }
                break;

            default:
                for (int i = -2; i < 3; i++)
                {
                    GenerateBullet(CalculateBulletDirection(i, direction));
                }
                break;
        }
    }

    /// <summary>
    /// バレット生成
    /// </summary>
    /// <param name="direction"></param>
    private void GenerateBullet(Vector2 direction)
    {
        Bullet bullet = Instantiate(bulletPrefab, gameManager.CharaController.transform.position, Quaternion.identity);  //第二引数をTransformの情報にしてしまうと親子関係が築かれてしまうのでここではpositionを指定する

        bullet.transform.SetParent(temporaryObjectsPlace);

        bullet.Shoot(direction);
    }

    /// <summary>
    /// 弾の方向を計算する
    /// </summary>
    private Vector2 CalculateBulletDirection(int i, Vector2 direction)
    {
        //角度の補正値の決定
        float offsetAngle = i * offsetDegrees;

        //上の補正値を回転させた回転情報を作る(Quaternion.Euler(a, b, c);で、x、y、z軸周りにそれぞれa、b、c度回転する)
        Quaternion offsetRotation = Quaternion.Euler(0, 0, offsetAngle);

        //上記の回転情報に、directionを掛けることで、ここで弾の方向のベクトルが決まる(directionをoffsetRotationだけ変える)  //TODO わからない
        Vector2 offsetDirection = offsetRotation * direction;

        return offsetDirection;
    }
}
