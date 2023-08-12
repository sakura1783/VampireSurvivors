using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet1 : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 3;

    //[SerializeField] private float duration = 1.0f;

    [SerializeField] private Transform target;  //旋回の軸となるターゲット

    private float direction = -1;  //-1で時計回り、1で半時計回り


    void Update()
    {
        if (!target)
        {
            return;
        }

        Turn();
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpBullet1(CharaController charaController)
    {
        target = charaController.transform;

        //Spin();
    }

    /// <summary>
    /// ゲームオブジェクトをぐるぐると回転させる
    /// </summary>
    //private void Spin()
    //{
    //    transform.DORotate(new Vector3(0, 0, 360), duration, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).SetLink(gameObject);
    //}

    /// <summary>
    /// ゲームオブジェクトを旋回させる
    /// </summary>
    private void Turn()
    {
        //プレイヤーを中心に円運動
        transform.RotateAround(target.transform.position, Vector3.forward, direction * rotateSpeed * Time.deltaTime);  //RotateAround(中心となるワールド座標、回転軸、回転角度)
    }
}
