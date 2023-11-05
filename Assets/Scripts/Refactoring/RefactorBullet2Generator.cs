using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

/// <summary>
/// 追尾弾
/// </summary>
public class RefactorBullet2Generator : BulletGeneratorBase
{
    //private int bulletLevel = 1;
    //public int BulletLevel
    //{
    //    get => bulletLevel;
    //    set => bulletLevel = value;
    //}

    //private Transform temporaryObjectsPlace;

    private EnemyController target;  //追尾対象の敵


    /// <summary>
    /// 弾生成
    /// </summary>
    public override async void GenerateBullet<T>(Vector2 direction, T t)
    {
        //レベルに応じて生成する弾の数を変える
        for (int i = 0; i < bulletLevel; i++)
        {
            //メソッドがUniTaskを返す場合、下のように待機できる
            await FindNearestEnemy();

            //ターゲットが見つかった場合のみ、バレット生成
            if (target)
            {
                //プールから弾を取り出す。ない場合は新しく生成
                BulletBase bullet = GetBullet(transform.position, Quaternion.identity);

                bullet.transform.SetParent(temporaryObjectsPlace);

                //targetの情報を弾に渡す
                bullet.SetUpBullet(target);

                await UniTask.WaitForSeconds(0.2f);
            }
        }
    }

    //public override void GenerateBullet(Vector2 direction) { }

    //public override void GenerateBullet<T>(Vector2 direction, T t) { }

    /// <summary>
    /// 一番近い敵を見つける
    /// </summary>
    /// <returns></returns>
    private async UniTask FindNearestEnemy()
    {
        target = null;

        //敵が存在している場合のみ処理を行う
        if (GameData.instance.enemiesList.Count > 0)
        {
            //敵との距離が近い順にリストを並び替え
            var sortedEnemiesList = GameData.instance.enemiesList.OrderBy(enemy => Vector2.Distance(transform.position, enemy.transform.position));

            foreach (var enemy in sortedEnemiesList)
            {
                //enemyがtargetListに含まれている場合はスキップ(各弾の追尾対象は被らない)
                if (GameData.instance.targetList.Contains(enemy))
                {
                    continue;
                }

                target = enemy;

                //ターゲットが見つかり次第、ループ終了
                break;
            }

            if (target)
            {
                //追尾対象をターゲットリストに追加
                GameData.instance.targetList.Add(target);
            }
        }

        //1フレームだけ待機
        await UniTask.DelayFrame(1);
    }
}
