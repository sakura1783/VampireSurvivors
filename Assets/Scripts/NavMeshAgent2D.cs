using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgent2D : MonoBehaviour
{
    [Header("Steering")]
    public float speed = 1.0f;
    public float stoppingDistance = 0;

    [HideInInspector]//常にUnityエディタから非表示
    private Vector2 trace_area = Vector2.zero;

    public Vector2 destination
    {
        get { return trace_area; }
        set
        {
            trace_area = value;
            Trace(transform.position, value);
        }
    }
    public bool SetDestination(Vector2 target)
    {
        destination = target;
        return true;
    }

    private void Trace(Vector2 current, Vector2 target)
    {
        if (Vector2.Distance(current, target) <= stoppingDistance)
        {
            return;
        }

        // NavMesh に応じて経路を求める
        NavMeshPath path = new NavMeshPath();
        if (!NavMesh.CalculatePath(current, target, NavMesh.AllAreas, path))
        {
            return;
        }
        //path.corners[0]は現在の敵の位置、path.corners[1]が次の目的地。
        //次の目的地とは、プレイヤーの位置ではなく、そこに行くまでの曲がり角の情報。そのため、障害物などの関係で複数の曲がり角がある場合、最も的に近い位置の曲がり角の情報。
        Vector2 corner = path.corners[0];

        if (Vector2.Distance(current, corner) <= 0.05f)
        {
            if (path.corners.Length < 2)
            {
                corner = path.corners[0];

                return;
            }

            corner = path.corners[1];
        }

        transform.position = Vector2.MoveTowards(current, corner, speed * Time.deltaTime);
    }
}
