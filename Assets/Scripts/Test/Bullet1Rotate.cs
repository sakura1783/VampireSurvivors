using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet1Rotate : MonoBehaviour
{
    [SerializeField] private float duration = 1.0f;


    void Start()
    {
        Spin();
    }

    /// <summary>
    /// ゲームオブジェクトをぐるぐると回転させる
    /// </summary>
    private void Spin()
    {
        transform.DORotate(new Vector3(0, 0, 360), duration, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).SetLink(gameObject);
    }
}
