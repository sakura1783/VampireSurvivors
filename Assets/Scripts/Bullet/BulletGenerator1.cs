using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator1 : MonoBehaviour
{
    [SerializeField] private Bullet1 bulletPrefab;

    private CharaController charaController;


    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpBulletGenerator1()
    {
        charaController = GetComponent<CharaController>();
    }

    /// <summary>
    /// バレット生成準備
    /// </summary>
    public void PrepareGenerateBullet()
    {
        GenerateBullet();
    }

    /// <summary>
    /// バレットの生成
    /// </summary>
    private void GenerateBullet()
    {
        Bullet1 bullet = Instantiate(bulletPrefab, transform);

        bullet.SetUpBullet1(charaController);
    }
}
