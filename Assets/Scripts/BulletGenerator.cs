using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;

    [SerializeField] private Transform temporaryObjectsPlace;

    [SerializeField] private GameManager gameManager;  //CharaController取得用


    /// <summary>
    /// バレット生成の準備。レベルに応じてバレットの数を増やす
    /// </summary>
    /// <param name="direction"></param>
    public void PrepareGenerateBullet(Vector2 direction)
    {
        switch (gameManager.CharaController.level)
        {
            case 1:
                GenerateBullet(direction);
                break;
            case 2:
                for (int i = -1; i < 2; i++)
                {
                    if (i == 0)
                    {
                        //以下の処理をスキップして、次のforeach文の処理に移る
                        continue;
                    }

                    //補正値の計算
                    float offset = i * 0.125f;

                    GenerateBullet(new Vector2((direction.x + offset), direction.y));
                }
                break;
        }
    }

    /// <summary>
    /// バレット生成
    /// </summary>
    /// <param name="direction"></param>
    private void GenerateBullet(Vector2 direction)
    {
        Bullet bullet = Instantiate(bulletPrefab, gameManager.CharaController.transform);

        bullet.transform.SetParent(temporaryObjectsPlace);

        bullet.Shoot(direction);
    }
}
