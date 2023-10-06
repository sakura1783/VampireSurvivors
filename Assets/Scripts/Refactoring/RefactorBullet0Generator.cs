using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefactorBullet0Generator : BulletGeneratorBase
{
    private int bulletLevel = 1;
    public int BulletLevel
    {
        get => bulletLevel;
        set => bulletLevel = value;
    }

    [SerializeField] private Transform temporaryObjectsPlace;

    [SerializeField] private float offsetDegrees;  //角度の基準値(各バレット同士の角度間隔)


    /// <summary>
    /// バレット生成
    /// </summary>
    /// <param name="direction"></param>
    public override void GenerateBullet(Vector2 direction)
    {
        for (int i = 0; i < bulletLevel; i++)
        {
            //プールから弾を取得。ない場合は新しく生成
            BulletBase bullet = GetBullet(transform.position, Quaternion.identity);

            bullet.transform.SetParent(temporaryObjectsPlace);

            bullet.Shoot(CalculateBulletDirection(i, direction));
        }
    }

    public override void GenerateBullet<T>(Vector2 direction, T t) { }

    /// <summary>
    /// 弾の方向を計算する
    /// </summary>
    /// <param name="count"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    private Vector2 CalculateBulletDirection(int count, Vector2 direction)
    {
        //角度の補正値の決定
        float offsetAngle = count * offsetDegrees;

        //上の補正値を回転させた回転情報を作る(Quaternion.Euler(a, b, c);で、x, y, z軸周りにそれぞれa, b, c度回転する)
        Quaternion offsetRotation = Quaternion.Euler(0, 0, offsetAngle);

        //上の回転情報に、directionを掛けることで、弾の方向ベクトルが決まる(Unityの場合、Quaternion * Vector3をすると、Vector3をQuaternionで回転させた座標が得られる。directionをoffsetRotationだけ変える。directionの方向を維持しつつ、Z軸だけを回転させた情報を持っている新しいベクトルを作成している。)
        Vector2 offsetDirection = offsetRotation * direction;

        return offsetDirection;
    }
}
