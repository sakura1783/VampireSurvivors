using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザー
/// </summary>
public class RefactorBullet3Generator : BulletGeneratorBase
{
    //private int bulletLevel = 1;
    //public int BulletLevel
    //{
    //    get => bulletLevel;
    //    set => bulletLevel = value;
    //}

    private Transform temporaryObjectsPlace;

    private Vector3 lineDirection;

    [SerializeField] private float bulletLine;  //バレットたちが配置される直線

    [SerializeField] private bool isDebugDrawRayOn;  //デバッグ用の描画処理をするかどうか


    protected override void Update()
    {
        base.Update();

        if (!isDebugDrawRayOn)
        {
            return;
        }

        //傾けた直線を描画などで使用する
        Debug.DrawRay(charaController.transform.position, lineDirection, Color.red);
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="charaController"></param>
    public override void SetUpBulletGenerator(CharaController charaController, BulletDataSO.BulletData bulletData, Transform place)
    {
        base.SetUpBulletGenerator(charaController, bulletData);

        temporaryObjectsPlace = this.charaController.temporaryObjectsPlace;
    }

    /// <summary>
    /// 弾生成
    /// </summary>
    public override void GenerateBullet(Vector2 direction)
    {
        //レベルに応じて、直線の長さを調整する(大きくしないとバレット同士の間隔が詰まってしまう)。/2はバレット同士の間隔は広げつつ、急激に広がりすぎないようにするための調整。(適宜、計算方法を検討する)
        float currentBulletLine = bulletLine / 2 * bulletLevel;

        //directionの情報をもとに、プレイヤーの向きに対する角度(ラジアン)を求め(Mathf.Atan2)、その角度を度数に変換し(* Mathf.Rad2Deg)、プレイヤーの向きに合わせて傾ける角度を求める。
        //角度の初期値は90度(時計の針の3時)を0として始まるので、水平にするには90をたす
        float tilt = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;  //tilt = 傾ける

        //Quaternion.Eulerを使い、プレイヤーの向きに合わせて傾けた回転角度を作成
        Quaternion rotation = Quaternion.Euler(0, 0, tilt);

        //Quaternion型とVector3型を計算することで、回転情報を持つVector3型が求められる(手裏剣の時にも使っている処理)
        //中心点(プレイヤーの位置)を基準に、プレイヤーの向きに応じて傾いた直線ベクトル(方向)を算出。rotation(傾けた回転角度) * Vector3.right(ワールド座標での水平(x軸)方向)
        lineDirection = rotation * Vector3.right;

        //レベルに応じて、生成する弾の数を変える
        for (int i = 0; i < bulletLevel; i++)
        {
            float angle = 0;

            //バレットのレベルが2以上なら、角度間隔を調整する
            if (bulletLevel > 1)
            {
                //直線上の角度間隔の計算
                float t = (float)i / (bulletLevel - 1);

                //直線上に用意する弾の数で、各弾の角度間隔を算出(Mathf.Lerp([マイナス側の半径値], [プラス側の半径値], [直線上の等間隔数]))
                //Mathf.Lerp(最小値, 最大値, 最小値~最大値の何割になる値を使うか(0~1))。i = 0の時は(-5, 5, 0)で、これは、-5と5の範囲を0~100%で表現するとした時、0%の時の値を計算して、というもの。0%の時は、-5になる(Mathf.Lerp(-5, 5, 0) = -5)。
                angle = Mathf.Lerp(-currentBulletLine / 2, currentBulletLine / 2, t);
            }

            //angle * lineDirectionをすることで、傾いた状態での直線上の等間隔の値が算出できる
            Vector2 distance = angle * lineDirection;

            //プールから取り出す。ない場合は新しく生成する
            BulletBase bullet = GetBullet((Vector2)transform.position + distance, rotation);

            bullet.transform.SetParent(temporaryObjectsPlace);

            bullet.Shoot(direction);
        }
    }

    public override void GenerateBullet() { }

    public override void GenerateBullet<T>(Vector2 direction, T t) { }
}
