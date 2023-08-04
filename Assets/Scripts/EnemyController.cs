using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator enemyAnim;

    [SerializeField] private CharaController charaController;

    [SerializeField] private NavMeshAgent2D navMeshAgent2D;


    void Start()
    {
        SetUpEnemyController();
    }

    void Update()
    {
        navMeshAgent2D.destination = charaController.transform.position;

        ChangeAnimDirection();
    }

    public void SetUpEnemyController()
    {
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
}
