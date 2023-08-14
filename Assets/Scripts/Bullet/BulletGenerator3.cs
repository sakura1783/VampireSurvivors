using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザー
/// </summary>
public class BulletGenerator3 : MonoBehaviour
{
    [SerializeField] private Bullet3 bulletPrefab;

    [SerializeField] private Transform temporaryObjectsPlace;

    [SerializeField] private CharaController charaController;

    [SerializeField] private float bulletDistance;  //バレット間の距離


    /// <summary>
    /// バレット生成準備
    /// </summary>
    public void PrepareGenerateBullet(Vector2 direction)
    {
        switch (charaController.level)
        {
            case 1:
                GenerateBullet(direction, CalculateGeneratePos(0));
                break;

            case 2:
                for (int i = -1; i < 2; i += 2)
                {
                    GenerateBullet(direction, CalculateGeneratePos(i));
                }
                break;

            case 3:
                for (int i = -1; i < 2; i++)
                {
                    GenerateBullet(direction, CalculateGeneratePos(i));
                }
                break;

            case 4:
                for (int i = -3; i < 4; i += 2)
                {
                    GenerateBullet(direction, CalculateGeneratePos(i));
                }
                break;

            default:
                for (int i = -2; i < 3; i++)
                {
                    GenerateBullet(direction, CalculateGeneratePos(i));
                }
                break;
        }
    }

    /// <summary>
    /// バレット生成
    /// </summary>
    private void GenerateBullet(Vector2 direction, Vector2 generatePos)
    {
        Bullet3 bullet = Instantiate(bulletPrefab, (Vector2)transform.position + generatePos, Quaternion.identity);

        //Bullet3 bullet = Instantiate(bulletPrefab);
        //Bullet3 bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Debug.Log($"②バレットが生成された位置：{bullet.transform.position}");

        bullet.transform.SetParent(temporaryObjectsPlace);

        //飛んでいく方向と向きを同期
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        Debug.Log($"③回転：{bullet.transform.rotation}");

        //bullet.transform.position = (Vector2)transform.position + generatePos;

        bullet.Shoot(direction);
    }

    /// <summary>
    /// バレットの生成位置を計算する
    /// </summary>
    private Vector2 CalculateGeneratePos(int count)
    {
        //float posX = -3;

        //キャラの向きによって生成する位置を変える
        //float posX = charaController.transform.GetChild(0).localScale.x > 0 ? -3 : 3;

        //float posY = count * bulletDistance;

        float posX = 0;
        float posY = 0;

        //再生されているアニメによって生成する位置と間隔を変える
        //switch (charaController.AnimatorClipInofo[0].clip.name)
        switch (charaController.CharaAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name)
        {
            case "walk_down":
                posX = count * bulletDistance;
                posY = -3;
                Debug.Log("①walk_downの分岐です");
                break;

            case "walk_up":
                posX = count * bulletDistance;
                posY = 3;
                Debug.Log("①walk_upの分岐です");
                break;

            case "walk_side":
                posX = charaController.transform.GetChild(0).localScale.x > 0 ? -3 : 3;
                posY = count * bulletDistance;
                Debug.Log("①walk_sideの分岐です");
                break;

                //case "walk_side":
                //    posX = -3;
                //    posY = count * bulletDistance;
                //    break;

                //case "walk_side2":
                //    posX = 3;
                //    posY = count * bulletDistance;
                //    break;
        }

        Vector2 offsetPos = new Vector2(posX, posY);

        return offsetPos;
    }
}
