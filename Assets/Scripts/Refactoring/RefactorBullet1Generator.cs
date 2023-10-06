using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 手裏剣
/// </summary>
public class RefactorBullet1Generator : BulletGeneratorBase
{
    private int bulletLevel = 1;
    public int BulletLevel
    {
        get => bulletLevel;
        set => bulletLevel = value;
    }

    private List<BulletBase> bulletList = new();


    /// <summary>
    /// オブジェクトプールと継承を使った弾の生成
    /// </summary>
    /// <param name="generatePos"></param>
    public override void GenerateBullet(Vector2 direction)
    {
        //プールに弾を全て戻し、リストからも削除
        DestroyBullets();

        //弾の数に基づいて、各弾の発射角度を計算
        float offsetDegrees = 360 / bulletLevel;

        //弾のレベルに応じて生成する数を変更
        for (int i = 0; i < bulletLevel; i++)
        {
            //回転情報にdirectionをかけることで、ここで弾の方向のベクトルが決まる
            Vector2 offsetDirection = Quaternion.Euler(0, 0, i * offsetDegrees) * direction;

            //オブジェクトの位置から一定距離離れた位置が計算される
            Vector2 offsetPos = (Vector2)transform.position + offsetDirection;

            //オブジェクトプールから取り出す。なければ新しく生成する
            BulletBase bullet = GetBullet(offsetPos, Quaternion.identity);

            bullet.transform.SetParent(charaController.transform);

            //弾の初期設定
            bullet.SetUpBullet(charaController);

            bulletList.Add(bullet);
        }
    }

    public override void GenerateBullet() { }

    public override void GenerateBullet<T>(Vector2 direction, T t) { }

    /// <summary>
    /// BulletGenerator1のDestroyObjectsFromListメソッドををオブジェクトプールを使って記述
    /// </summary>
    private void DestroyBullets()
    {
        foreach (var bullet in bulletList)
        {
            //破壊ではなくプールに戻す
            bullet.ReleaseBullet();
        }

        bulletList.Clear();
    }
}
