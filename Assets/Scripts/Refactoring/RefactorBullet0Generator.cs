using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// デフォルト弾
/// </summary>
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
    /// 弾生成の準備
    /// </summary>
    /// <param name="direction"></param>
    public void PrepareGenerateBullet(Vector2 direction)
    {
        //switch (bulletLevel)
        //{
        //    case 1:
        //        GenerateBullet(direction);
        //        break;

        //    case 2:
        //        for (int i = -1; i < 2; i += 2)
        //        {
        //            GenerateBullet(CalculateBulletDirection(i, direction));
        //        }
        //        break;

        //    case 3:
        //        for (int i = -1; i < 2; i++)
        //        {
        //            GenerateBullet(CalculateBulletDirection(i, direction));
        //        }
        //        break;

        //    case 4:
        //        for (int i = -3; i < 4; i += 2)
        //        {
        //            GenerateBullet(CalculateBulletDirection(i, direction));
        //        }
        //        break;

        //    default:
        //        for (int i = -2; i < 3; i++)
        //        {
        //            GenerateBullet(CalculateBulletDirection(i, direction));
        //        }
        //        break;
        //}

        //上の処理の書き換え。計算式の仕組みを作ることで、処理の一元化を図る

        //上記の処理を計算式に置き換えた場合
        float totalSpreadAngle = offsetDegrees;

        for (int i = 0; i < bulletLevel; i++)
        {
            //発射角度を0でセット(生成する弾が一つの時はこのまま使う)
            float offsetAngle = 0;

            //生成する弾が二つ以上の場合
            if (bulletLevel > 1)
            {
                //弾の広がり全体の角度(扇形の中心角)を、発射する弾の数-1で割り、各弾1つあたりの角度間隔を調整して補正値とする。
                //例えば totalSpreadAngle=30 なら、弾2つの場合は30度、弾3つの場合は15度になる
                float angleStep = totalSpreadAngle / (bulletLevel - 1);

                //各弾の角度を計算する
                //-totalSpreadAngle/2 は弾の広がりの中心から左側の角度を表す。この中心から、弾の数だけangleStepを増加させることで、各弾の角度を決定する
                //この操作により、弾の広がりの範囲内に対して、弾同士の角度間隔を等間隔で弾を配置する。
                //例えば弾2つの場合は-15度と15度(直角三角形の底角2つの方向)、弾3つの場合は-15度、0度、15度(直角三角形の頂角と底角2つの方向)になる
                offsetAngle = -totalSpreadAngle / 2 + i * angleStep;
            }

            Debug.Log($"offsetAngleの値：{offsetAngle}");

            //第一引数は角度情報をそのまま渡す
            Vector2 offsetDirection = CalculateBulletDirection(offsetAngle, direction);

            GenerateBullet(offsetDirection);
        }
    }

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

            bullet.Shoot(direction);
        }
    }

    public override void GenerateBullet() { }

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

    /// <summary>
    /// 弾の方向を計算する(上記メソッドのオーバーロード。switch文を書き換えたのでこのメソッドを追加)
    /// </summary>
    /// <param name="offsetAngle"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    private Vector2 CalculateBulletDirection(float offsetAngle, Vector2 direction)
    {
        //弾の角度補正を表すQuaternionを作成する(Quaternion.Euler(a, b, c);で、x, y, z軸周りにそれぞれa, b, c度回転する)
        Quaternion offsetRotation = Quaternion.Euler(0, 0, offsetAngle);

        //directionベクトルをoffsetRotationで回転させることで、弾の新しい方向ベクトルを得る(UnityのQuaternion * Vector3演算子により、方向ベクトルを回転させた結果が得られる)
        return offsetRotation * direction;
    }
}
