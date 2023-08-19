using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザー
/// </summary>
public class BulletGenerator3 : MonoBehaviour
{
    public int bulletLevel = 1;

    private Bullet3 bulletPrefab;

    private Transform temporaryObjectsPlace;

    private CharaController charaController;

    private Vector3 lineDirection;

    //[SerializeField] private float bulletDistance = 0.3f;  //バレット間の距離
    [SerializeField] private float bulletLine = 0.3f;  //バレットたちが配置される直線

    [SerializeField] private bool isDebugDrawRayOn;  //デバッグ用のオンオフのスイッチ


    void Update()
    {
        if (!isDebugDrawRayOn)
        {
            return;
        }

        //傾けた直線などを描画などで使用する
        Debug.DrawRay(charaController.transform.position, lineDirection, Color.red);
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpBulletGenerator3(CharaController charaController)
    {
        this.charaController = charaController;

        bulletPrefab = this.charaController.bullet3Prefab;

        temporaryObjectsPlace = this.charaController.temporaryObjectsPlace;
    }

    /// <summary>
    /// バレット生成準備
    /// </summary>
    //public void PrepareGenerateBullet(Vector2 direction)
    //{
    //    switch (charaController.charaLevel)
    //    {
    //        case 1:
    //            GenerateBullet(direction, CalculateGeneratePos(0));
    //            break;

    //        case 2:
    //            for (int i = -1; i < 2; i += 2)
    //            {
    //                GenerateBullet(direction, CalculateGeneratePos(i));
    //            }
    //            break;

    //        case 3:
    //            for (int i = -1; i < 2; i++)
    //            {
    //                GenerateBullet(direction, CalculateGeneratePos(i));
    //            }
    //            break;

    //        case 4:
    //            for (int i = -3; i < 4; i += 2)
    //            {
    //                GenerateBullet(direction, CalculateGeneratePos(i));
    //            }
    //            break;

    //        default:
    //            for (int i = -2; i < 3; i++)
    //            {
    //                GenerateBullet(direction, CalculateGeneratePos(i));
    //            }
    //            break;
    //    }
    //}

    /// <summary>
    /// バレット生成準備
    /// </summary>
    public void PrepareGenerateBullet(Vector2 direction)
    {
        //レベルに応じて、直線の長さを調整する(レベルが上がるにつれてbulletLineが大きくなる(大きくならないとバレット同士がぎゅうぎゅうに詰まってしまう)。/2はバレット同士の間隔は広げつつ、急激に広がりすぎないようにするための調整)
        float currentBulletLine = bulletLine / 2 * bulletLevel;

        //directionの情報をもとに、プレイヤーの向きに関する角度(ラジアン)を求め(Mathf.Atan2)、その情報を度数に変換し(* Mathf.Rad2Deg)、プレイヤーの向きに合わせて傾ける角度を求める。角度の初期値は右90度(時計の針の3時)を0として始まるので、水平にするには90を足す
        float tilt = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;  //tilt = 傾ける

        //Quaternion.Eulerを使い、プレイヤーの向きに合わせて傾けた回転角度を作成
        Quaternion rotation = Quaternion.Euler(0, 0, tilt);

        //Quaternion型とVector3型を計算することで、回転情報を持つVector3型が求められる(手裏剣の時にも使っている処理)
        //中心点(プレイヤーの位置)を基準に、プレイヤーの向きに応じて傾いた直線ベクトル(方向)を算出。rotation(傾けた回転角度) * Vector3.right(ワールド座標での水平(x軸)方向)
        lineDirection = rotation * Vector3.right;

        for (int i = 0; i < bulletLevel; i++)
        {
            //直線上の一つあたりの角度の初期値
            float angle = 0;

            //レベル2以上なら角度間隔を調整する
            if (bulletLevel > 1)
            {
                //直線上の等間隔数の計算
                float t = (float)i / (bulletLevel - 1);  //tは0から1の間をとる。これを用いることで、弾の間隔を均等にすることができる

                //直線上に用意する弾の数で、各弾の角度間隔を算出(Mathf.Lerp([マイナス側の半径値], [プラス側の半径値], [直線上の等間隔数]))
                //Mathf.Lerp(最小値, 最大値, 最小値~最大値の何割になる値を使うか(0~1))。i = 0の時は(-5, 5, 0)で、これは、-5と5の範囲を0~100%で表現するとした時、0%の時の値を計算して、というもの。0%の時は、-5になる(Mathf.Lerp(-5, 5, 0) = -5)。
                angle = Mathf.Lerp(-currentBulletLine / 2, currentBulletLine / 2, t);  //-currentBulletLine/2からcurrentBulletLine/2の範囲で角度を補完
            }

            //angle * lineDirectionをすることで、傾いた状態での直線上の等間隔の値が算出できる
            Vector2 distance = angle * lineDirection;

            GenerateBullet(direction, distance, rotation);
        }
    }

    /// <summary>
    /// バレット生成
    /// </summary>
    //private void GenerateBullet(Vector2 direction, Vector2 generatePos)
    //{
    //    Bullet3 bullet = Instantiate(bulletPrefab, (Vector2)transform.position + generatePos, Quaternion.identity);

    //    //Bullet3 bullet = Instantiate(bulletPrefab);
    //    //Bullet3 bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    //    Debug.Log($"②バレットが生成された位置：{bullet.transform.position}");

    //    bullet.transform.SetParent(temporaryObjectsPlace);

    //    //飛んでいく方向と向きを同期
    //    bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
    //    Debug.Log($"③回転：{bullet.transform.rotation}");

    //    //bullet.transform.position = (Vector2)transform.position + generatePos;

    //    bullet.Shoot(direction);
    //}

    /// <summary>
    /// バレット生成
    /// </summary>
    private void GenerateBullet(Vector2 direction, Vector2 generatePos, Quaternion rotation)
    {
        Bullet3 bullet = Instantiate(bulletPrefab, (Vector2)transform.position + generatePos, rotation);

        bullet.transform.SetParent(temporaryObjectsPlace);

        bullet.Shoot(direction);
    }

    /// <summary>
    /// バレットの生成位置を計算する
    /// </summary>
    //private Vector2 CalculateGeneratePos(Vector2 direction, int count)
    //{
    //    //float posX = -3;

    //    //キャラの向きによって生成する位置を変える
    //    //float posX = charaController.transform.GetChild(0).localScale.x > 0 ? -3 : 3;

    //    //float posY = count * bulletDistance;

    //    float posX = 0;
    //    float posY = 0;

    //    //再生されているアニメによって生成する位置と間隔を変える
    //    //switch (charaController.AnimatorClipInofo[0].clip.name)
    //    switch (charaController.CharaAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name)
    //    {
    //        case "walk_down":
    //            posX = count * bulletDistance;
    //            posY = -3;
    //            Debug.Log("①walk_downの分岐です");
    //            break;

    //        case "walk_up":
    //            posX = count * bulletDistance;
    //            posY = 3;
    //            Debug.Log("①walk_upの分岐です");
    //            break;

    //        case "walk_side":
    //            posX = charaController.transform.GetChild(0).localScale.x > 0 ? -3 : 3;
    //            posY = count * bulletDistance;
    //            Debug.Log("①walk_sideの分岐です");
    //            break;

    //            //case "walk_side":
    //            //    posX = -3;
    //            //    posY = count * bulletDistance;
    //            //    break;

    //            //case "walk_side2":
    //            //    posX = 3;
    //            //    posY = count * bulletDistance;
    //            //    break;
    //    }

    //    Debug.Log($"GetCurrentAnimatorClipInfoの値：{charaController.CharaAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name}");

    //    Vector2 offsetPos = new Vector2(posX, posY);

    //    return offsetPos;
    //}

    /// <summary>
    /// 回転の計算
    /// </summary>
    /// <param name="charaController"></param>
    //public void CalculateRotation(CharaController charaController)
    //{
    //    Vector3 direction = charaController.Direction;

    //    //directionの方向を元に傾ける角度を求める
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90; //角度に置き換える計算にはMathf.Atan2とMathf.Rad2Degを利用できる。右端が0で始まるので、水平にするには90足す

    //    //Quaternion.Eulerで傾けた角度を作成
    //    Quaternion rotation = Quaternion.Euler(0, 0, angle);

    //    //中心点を基準に直線を傾ける
    //    Vector3 lineDirection = rotation * Vector3.right;

    //    transform.rotation = rotation;
    //}

    /// <summary>
    /// バレットの生成位置を計算
    /// </summary>
    //public float CalculateGeneratePos(CharaController charaController, int generateCount)
    //{
    //    Vector3 direction = charaController.Direction;

    //    //directionの方向を元に傾ける角度を求める
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90; //角度に置き換える計算にはMathf.Atan2とMathf.Rad2Degを利用できる。右端が0で始まるので、水平にするには90足す

    //    //Quaternion.Eulerで傾けた角度を作成
    //    Quaternion rotation = Quaternion.Euler(0, 0, angle);

    //    //中心点を基準に直線を傾ける
    //    Vector3 lineDirection = rotation * Vector3.right;

    //    transform.rotation = rotation;

    //    //Mathf.Lerpを使ってバレットを等間隔に配置(Mathf.Lerp(マイナス側の半径値, プラス側の半径値, 直線上の等間隔数))
    //    //float distance = Mathf.Lerp(-20, 20, generateCount);

    //    //return distance;
    //}
}
