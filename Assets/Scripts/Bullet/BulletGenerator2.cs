using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 追尾弾
/// </summary>
public class BulletGenerator2 : MonoBehaviour
{
    public int bulletLevel = 1;

    private Bullet2 bulletPrefab;

    Transform temporaryObjectsPlace;

    private CharaController charaController;


    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpBulletGenerator2(CharaController charaController)
    {
        this.charaController = charaController;

        bulletPrefab = this.charaController.bullet2Prefab;

        temporaryObjectsPlace = this.charaController.temporaryObjectsPlace;
    }

    /// <summary>
    /// バレット生成の準備
    /// </summary>
    public IEnumerator PrepareGenerateBullet()
    {
        switch (bulletLevel)
        {
            case 1:
                yield return StartCoroutine(GenerateBullet());
                break;

            case 2:
                for (int i = 0; i < 2; i++)
                {
                    yield return StartCoroutine(GenerateBullet());
                }
                break;

            case 3:
                for (int i = 0; i < 3; i++)
                {
                    yield return StartCoroutine(GenerateBullet());
                }
                break;

            case 4:
                for (int i = 0; i < 4; i++)
                {
                    yield return StartCoroutine(GenerateBullet());
                }
                break;

            default:
                for (int i = 0; i < 5; i++)
                {
                    yield return StartCoroutine(GenerateBullet());
                }
                break;
        }
    }

    /// <summary>
    /// バレット生成
    /// </summary>
    private IEnumerator GenerateBullet()
    {
        Bullet2 bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        bullet.transform.SetParent(temporaryObjectsPlace);

        bullet.FindNearestEnemy(charaController.Direction);

        yield return new WaitForSeconds(0.2f);
    }
}
