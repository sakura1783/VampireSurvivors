using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 手裏剣
/// </summary>
public class RefactorBullet1 : BulletBase
{
    [SerializeField] private float rotateSpeed;

    [SerializeField] private Transform target;  //旋回(回転)の軸となるターゲット

    private float direction = -1;  //-1で時計回り、1で反時計回り


    void Update()
    {
        if (!target)
        {
            return;
        }

        Turn();
    }

    public override void SetUpBullet<T>(T charaController)
    {
        base.SetUpBullet(charaController);

        //T型の型が判明できていないため、if文とis演算子を使ってキャストのチェックとキャストをする([変数名] is [キャストできるか確認する型名] [キャストできた場合の変数名])
        if (charaController is CharaController chara)
        {
            target = chara.transform;
        }
        else
        {
            Debug.Log("CharaControllerが取得できませんでした");
        }
    }

    public override void Shoot(Vector2 direction) { }  //このクラスでは利用しないが、BulletBaseでShootメソッドがabstractで定義されているため、必ず定義しなければいけない

    /// <summary>
    /// ゲームオブジェクトを旋回させる
    /// </summary>
    private void Turn()
    {
        //プレイヤーを中心に円運動。RotateAround(中心となるワールド座標, 回転軸, 回転角度)
        transform.RotateAround(target.transform.position, Vector3.forward, direction * rotateSpeed * Time.deltaTime);
    }
}
