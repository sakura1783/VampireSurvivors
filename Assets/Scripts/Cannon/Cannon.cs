using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private CharaController charaController;

    [SerializeField] private CannonBullet bulletPrefab;

    [SerializeField] private Transform temporaryObjectsPlace;

    [SerializeField] private float attackInterval;

    private float timer;


    /// <summary>
    /// バレット生成準備
    /// </summary>
    public IEnumerator PrepareGenerateBullet()
    {
        while (true)
        {
            //ポップアップ表示中は新たなバレットを生成しない
            if (charaController.levelupPop.isDisplayPopUp)
            {
                yield return null;

                continue;
            }

            //yield return new WaitForSeconds(attackInterval);

            timer += Time.deltaTime;

            if (timer >= attackInterval)
            {
                timer = 0;

                Vector2 direction = (charaController.transform.position - transform.position).normalized;

                GenerateBullet(direction);
            }

            yield return null;
        }
    }

    /// <summary>
    /// バレット生成
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="rotation"></param>
    //private void GenerateBullet(Vector2 direction, Quaternion rotation)
    //{
    //    CannonBullet bullet = Instantiate(bulletPrefab, transform.position, rotation);

    //    bullet.transform.SetParent(temporaryObjectsPlace);

    //    bullet.Shoot(charaController, direction);
    //}

    /// <summary>
    /// バレット生成
    /// </summary>
    private void GenerateBullet(Vector2 direction)
    {
        CannonBullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        bullet.transform.SetParent(temporaryObjectsPlace);

        bullet.Shoot(charaController, direction);
    }

    /// <summary>
    /// バレットの回転角度を計算
    /// </summary>
    //private Quaternion CalculateBulletRotation(Vector2 direction)
    //{
    //    //directionの情報をもとに、プレイヤーの向きに関する角度(ラジアン)を求め(Mathf.Atan2)、その情報を度数に変換し(* Mathf.Rad2Deg)、プレイヤーの向きに合わせて傾ける角度を求める
    //    float tilt = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    //    //Quaternion.Eulerを使い、プレイヤーの向きに合わせて傾けた回転角度を作成
    //    Quaternion rotation = Quaternion.Euler(0, 0, tilt);

    //    return rotation;
    //}
}
