using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 凍結魔法
/// </summary>
public class BulletGenerator5 : MonoBehaviour
{
    [SerializeField] private Bullet5 bulletPrefab;

    [SerializeField] private Transform temporaryObjectsPlace;

    [SerializeField] private CharaController charaController;

    [SerializeField] private float offsetDegrees = 5;  //バレット同士の角度間隔


    /// <summary>
    /// バレット生成の準備
    /// </summary>
    /// <param name="direction"></param>
    public void PrepareGenerateBullet(Vector2 direction)
    {
        switch (charaController.level)
        {
            case 1:
                GenerateBullet(direction);
                break;

            case 2:
                for (int i = -1; i < 2; i += 2)
                {
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
    private void GenerateBullet(Vector2 direction)
    {
        Bullet5 bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        bullet.transform.SetParent(temporaryObjectsPlace);

        bullet.Shoot(direction, temporaryObjectsPlace);
    }

    /// <summary>
    /// バレットの方向を計算
    /// </summary>
    /// <param name="count"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    private Vector2 CalculateBulletDirection(int count, Vector2 direction)
    {
        //角度の補正値の決定
        float offsetAngle = count * offsetDegrees;

        //上の補正値を回転させた回転情報を作る
        Quaternion offsetRotation = Quaternion.Euler(0, 0, offsetAngle);

        //上記の回転情報に、directionを掛けることで、ここで弾の方向のベクトルが決まる
        Vector2 bulletDirection = offsetRotation * direction;

        return bulletDirection;
    }
}
