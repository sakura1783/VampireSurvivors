using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefactorBullet5Generator : BulletGeneratorBase
{
    private int bulletLevel = 1;
    public int BulletLevel
    {
        get => bulletLevel;
        set => bulletLevel = value;
    }

    private Transform temporaryObjectsPlace;

    [SerializeField] private float offsetDegrees;  //バレット同士の角度間隔


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="charaController"></param>
    public override void SetUpBulletGenerator(CharaController charaController, BulletDataSO.BulletData bulletData, Transform place = null)
    {
        base.SetUpBulletGenerator(charaController, bulletData);

        temporaryObjectsPlace = this.charaController.temporaryObjectsPlace;
    }

    /// <summary>
    /// 弾生成の準備
    /// </summary>
    /// <param name="direction"></param>
    public void PrepareGenerateBullet(Vector2 direction)
    {
        //TODO switch文無くす
        switch (bulletLevel)
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
    /// 弾生成
    /// </summary>
    /// <param name="direction"></param>
    public override void GenerateBullet(Vector2 direction)
    {
        //プールから取り出す。ない場合は新しく生成
        BulletBase bullet = GetBullet(transform.position, Quaternion.identity);

        bullet.transform.SetParent(temporaryObjectsPlace);

        bullet.SetUpBullet(temporaryObjectsPlace);

        bullet.Shoot(direction);
    }

    public override void GenerateBullet() { }

    public override void GenerateBullet<T>(Vector2 direction, T t) { }

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
