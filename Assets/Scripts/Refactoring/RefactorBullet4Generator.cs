using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// 雷
/// </summary>
public class RefactorBullet4Generator : BulletGeneratorBase
{
    private int bulletLevel = 1;
    public int BulletLevel
    {
        get => bulletLevel;
        set => bulletLevel = value;
    }

    [SerializeField] private float attackInterval;
    public float AttackInterval
    {
        get => attackInterval;
        set => attackInterval = value;
    }

    private Transform temporaryObjectsPlace;

    private EnemyController target;

    private Vector2 offsetPos;  //最終的な生成位置


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
    public override async void GenerateBullet()
    {
        await CalculateGeneratePos();

        //攻撃対象がいる場合のみバレットを生成
        if (target)
        {
            //プールから弾を取り出す。ない場合は新しく生成
            BulletBase bullet = GetBullet(offsetPos, Quaternion.identity);

            bullet.transform.SetParent(temporaryObjectsPlace);

            //TODO 括弧にRefactorBullet4のdestroyTime変数を指定したい
            //bullet.ReleaseBullet(

            //リストからtargetを削除
            GameData.instance.targetList.Remove(target);
        }
    }

    public override void GenerateBullet(Vector2 direction) { }

    public override void GenerateBullet<T>(Vector2 direction, T t) { }

    /// <summary>
    /// 弾の生成位置を計算
    /// </summary>
    /// <returns></returns>
    private async UniTask CalculateGeneratePos()
    {
        await FindTarget();

        if (target)
        {
            offsetPos = new Vector2(target.transform.position.x, target.transform.position.y + 3.5f);
        }
    }

    /// <summary>
    /// ランダムな敵を攻撃対象に設定
    /// </summary>
    /// <returns></returns>
    private async UniTask FindTarget()
    {
        target = null;

        //敵が存在していて、かつ、攻撃対象に設定可能な敵がいる場合
        if (GameData.instance.enemiesList.Count > 0 && GameData.instance.enemiesList.Count == GameData.instance.targetList.Count)
        {
            //do_while文 <= do{ 条件式がtrueの間繰り返される処理 }while(条件式)。この場合、targetListにtargetが含まれている間、do{}内の処理を繰り返す
            do
            {
                int randomNo = Random.Range(0, GameData.instance.enemiesList.Count);

                target = GameData.instance.enemiesList[randomNo];

            } while (GameData.instance.targetList.Contains(target));

            GameData.instance.targetList.Add(target);
        }

        await UniTask.DelayFrame(1);
    }

    /// <summary>
    /// レベルに応じて攻撃のインターバル時間を変える
    /// </summary>
    public void SetAttackIntervalByLevel()
    {
        switch (bulletLevel)
        {
            case 1:
                attackInterval = 3.0f;
                break;

            case 2:
                attackInterval = 2.5f;
                break;

            case 3:
                attackInterval = 2.0f;
                break;

            case 4:
                attackInterval = 1.5f;
                break;

            case 5:
                attackInterval = 1.0f;
                break;

            default:
                attackInterval = 0.5f;
                break;
        }
    }
}
