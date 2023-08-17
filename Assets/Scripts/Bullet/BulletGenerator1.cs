using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 手裏剣
/// </summary>
public class BulletGenerator1 : MonoBehaviour
{
    public int bulletLevel = 1;

    private Bullet1 bulletPrefab;

    //[SerializeField] private float offsetDistance;

    private List<Bullet1> bullet1List = new();

    private CharaController charaController;


    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpBulletGenerator1(CharaController charaController)
    {
        this.charaController = charaController;

        bulletPrefab = this.charaController.bullet1Prefab;
    }

    /// <summary>
    /// バレット生成準備
    /// </summary>
    public void PrepareGenerateBullet(Vector2 direction)
    {
        switch (bulletLevel)
        {
            case 1:
                GenerateBullet(CalculateBulletDirection(0, 360, direction));
                break;

            case 2:
                DestroyObjectsFromList();
                for (int i = 0; i < 2; i++)
                {
                    GenerateBullet(CalculateBulletDirection(i, 180f, direction));
                }
                break;

            case 3:
                DestroyObjectsFromList();
                for (int i = 0; i < 3; i++)
                {
                    GenerateBullet(CalculateBulletDirection(i, 120f, direction));
                }
                break;

            case 4:
                DestroyObjectsFromList();
                for (int i = 0; i < 4; i++)
                {
                    GenerateBullet(CalculateBulletDirection(i, 90f, direction));
                }
                break;

            case 5:
                DestroyObjectsFromList();
                for (int i = 0; i < 5; i++)
                {
                    GenerateBullet(CalculateBulletDirection(i, 72f, direction));
                }
                break;

            case 6:
                DestroyObjectsFromList();
                for (int i = 0; i < 6; i++)
                {
                    GenerateBullet(CalculateBulletDirection(i, 60f, direction));
                }
                break;

            default:
                DestroyObjectsFromList();
                for (int i = 0; i < 7; i++)
                {
                    GenerateBullet(CalculateBulletDirection(i, 51.4f, direction));
                }
                break;
        }
    }

    /// <summary>
    /// バレットの生成
    /// </summary>
    private void GenerateBullet(Vector2 generatePos)
    {
        //Bullet1 bullet = Instantiate(bulletPrefab, (Vector2)transform.position + generatePos, Quaternion.identity);  //CalculateBulletDirectionの戻り値で自分の座標 + 修正値をもらっているが、そこにさらに自分の座標を足してしまっている
        Bullet1 bullet = Instantiate(bulletPrefab, generatePos, Quaternion.identity);

        bullet.transform.SetParent(charaController.transform);

        bullet.SetUpBullet1(charaController);

        bullet1List.Add(bullet);
    }

    /// <summary>
    /// bullet1Listの中の要素を全て消す
    /// </summary>
    private void DestroyObjectsFromList()
    {
        for (int i = 0; i < bullet1List.Count; i++)
        {
            Destroy(bullet1List[i].gameObject);
        }

        bullet1List.Clear();
    }

    /// <summary>
    /// バレットを生成する位置を計算する
    /// </summary>
    /// <param name="i"></param>
    /// <param name="offsetDegrees"></param>
    /// <returns></returns>
    private Vector2 CalculateBulletDirection(int count, float offsetDegrees, Vector2 direction)
    {
        //float offsetAngle = i * offsetDegrees;

        //Quaternion offsetRotation = Quaternion.Euler(0, 0, offsetAngle);

        //Vector2 offsetDirection = offsetRotation * direction;

        //回転情報に、directionを掛けることで、ここで弾の方向のベクトルが決まる
        Vector2 bulletDirection = Quaternion.Euler(0, 0, count * offsetDegrees) * direction;

        //Vector2 offsetPos = (Vector2)transform.position + offsetDirection * offsetDistance;  //オブジェクトの位置から一定距離離れた位置が計算される
        //Vector2 offsetPos = (Vector2)transform.position + direction;
        Vector2 offsetPos = (Vector2)transform.position + bulletDirection;

        return offsetPos;
    }
}
