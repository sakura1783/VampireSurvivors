using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 tapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Move(tapPos);
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move(Vector2 tapPos)
    {
        transform.position = Vector2.MoveTowards(transform.position, tapPos, moveSpeed * Time.deltaTime);
    }
}
