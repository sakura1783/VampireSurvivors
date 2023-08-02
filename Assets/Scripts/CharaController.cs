using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    [SerializeField] private float limitPosX;
    [SerializeField] private float limitPosY;


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
        Vector2 newPos = Vector2.MoveTowards(transform.position, tapPos, moveSpeed * Time.deltaTime);

        //マップの範囲外にでないように制限をかける
        newPos.x = Mathf.Clamp(newPos.x, -limitPosX, limitPosX);
        newPos.y = Mathf.Clamp(newPos.y, -limitPosY, limitPosY);

        transform.position = newPos;
    }
}
