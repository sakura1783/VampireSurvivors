using System.Collections;
using UnityEngine;

/// <summary>
/// キャラの攻撃用クラス
/// </summary>
public class CharaAttack : MonoBehaviour, IChara
{
    [SerializeField] private float attackInterval;

    private BulletGenerator bulletGenerator;

    private bool isAttack;

    private Vector2 attackDirection;


    /// <summary>
    /// インターフェースで強制的に実装されるメソッド
    /// </summary>
    public void SetUpChara()
    {
        TryGetComponent(out bulletGenerator);

        Debug.Log("CharaAttack　設定完了");
    }

    /// <summary>
    /// 攻撃の準備
    /// </summary>
    public void ExecutePrepareAttack()
    {
        isAttack = true;

        StartCoroutine(PrepareAttack());

        Debug.Log("攻撃準備開始");
    }

    /// <summary>
    /// 攻撃準備
    /// </summary>
    private IEnumerator PrepareAttack()
    {
        while (true)
        {
            if (!isAttack)  //<= whileの内部にboolなどの分岐を作ると、攻撃の一時停止や再開に対応できる
            {
                yield return null;

                continue;
            }

            yield return new WaitForSeconds(attackInterval);

            Attack();
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
    {
        //Bullet bullet = Instantiate(bulletPrefab, transform);
        //bullet.transform.SetParent(temporaryObjectsPlace);
        //bullet.Shoot(direction);

        //上の処理をまとめる
        bulletGenerator.PrepareGenerateBullet(attackDirection);

        Debug.Log("攻撃");
    }

    /// <summary>
    /// CharaManagerのUpdateから最新の向きの情報をもらう
    /// </summary>
    /// <param name="newDirection"></param>
    public void UpdateAttackDirection(Vector2 newDirection)
    {
        attackDirection = newDirection;
    }

    /// <summary>
    /// 攻撃の一時停止
    /// </summary>
    public void PauseAttack()
    {
        isAttack = false;
    }

    /// <summary>
    /// 攻撃の再開
    /// </summary>
    public void ResumeAttack()
    {
        isAttack = true;
    }

    /// <summary>
    /// 攻撃のオンオフ(一時停止・再開)の切り替え
    /// </summary>
    public void ToggleAttack()
    {
        isAttack = !isAttack;

        Debug.Log(isAttack ? "攻撃再開" : "攻撃停止");
    }
}
