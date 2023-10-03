using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletGeneratorBase : MonoBehaviour, IGeneratable
{
    [SerializeField] protected float bulletSpeed;

    [SerializeField] protected BulletGeneratorBase bulletPrefab;

    protected CharaController charaController;

    //TODO オブジェクトプール機能を追加する


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="charaController"></param>
    public virtual void SetUpBulletGenerator(CharaController charaController)
    {
        this.charaController = charaController;

        Debug.Log("初期設定 完了");
    }

    public abstract void GenerateBullet(Vector2 direction);

    public abstract void GenerateBullet<T>(Vector2 direction, T t);
}
