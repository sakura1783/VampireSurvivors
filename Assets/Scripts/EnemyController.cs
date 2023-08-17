using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int attackPoint;

    public int exp;

    public bool isFrozen = false;  //凍っているかどうか。trueの場合は一定時間動きを止める

    private Animator enemyAnim;

    private CharaController charaController;

    [SerializeField] private NavMeshAgent2D navMeshAgent2D;


    void Update()
    {
        //ポップアップ表示中は動きを止める
        if (charaController.levelupPop.isDisplayPopUp)
        {
            return;
        }

        if (isFrozen)
        {
            return;
        }

        navMeshAgent2D.destination = charaController.transform.position;

        ChangeAnimDirection();
    }

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="charaController"></param>
    public void SetUpEnemyController(CharaController charaController)
    {
        this.charaController = charaController;

        transform.GetChild(0).TryGetComponent(out enemyAnim);
    }

    /// <summary>
    /// アニメ変更
    /// </summary>
    private void ChangeAnimDirection()
    {
        Vector2 direction = (charaController.transform.position - transform.position).normalized;

        enemyAnim.SetFloat("X", direction.x);
        enemyAnim.SetFloat("Y", direction.y);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet"))
        {
            //リストから削除
            GameData.instance.enemiesList.Remove(this);

            Destroy(gameObject);

            //Expの加算
            charaController.AddExp(exp);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //HPの更新
            charaController.UpdateHp(-attackPoint);

            //リストから削除
            GameData.instance.enemiesList.Remove(this);

            Destroy(gameObject);
        }
    }
}
