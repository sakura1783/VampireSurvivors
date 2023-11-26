using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//階段を登ったら、トンネル出入り用のコライダーのオン、オフを切り替える
public class MapManager : MonoBehaviour
{
    public bool isClimbedStairs = false;  //階段を登ったか、登ってないか(建物の上にいるか、下にいるか)

    //切り替えるコライダー
    [SerializeField] private Collider2D leftTunnelWall;
    [SerializeField] private Collider2D rightTunnelWall;
    [SerializeField] private Collider2D gateTopWall;
    [SerializeField] private TilemapCollider2D wall_switchTilemap;
    [SerializeField] private GameObject base_switchTilemap;


    /// <summary>
    /// 階段を登ったかどうか(建物の上にいるかどうか)を判定し、それに合わせて各地のコライダーのオンオフを切り替える
    /// (これを切り替えないと、意図しない場所でキャラが引っかかって通れない場所が発生したりする)
    /// </summary>
    /// <param name="isClimbedStairs">階段を登ったか、登ってないか(建物の上にいるか、下にいるか)</param>
    public void JudgeClimbedStairs()
    {
        //階段を登った(建物の上にいる)際の処理
        if (isClimbedStairs)
        {
            leftTunnelWall.enabled = false;
            rightTunnelWall.enabled = false;
            gateTopWall.enabled = true;
            wall_switchTilemap.enabled = true;
            //base_switchTilemap.SetActive(false);
        }

        //階段を降りた(建物の下にいる)際の処理
        if (!isClimbedStairs)
        {
            leftTunnelWall.enabled = true;
            rightTunnelWall.enabled = true;
            gateTopWall.enabled = false;
            wall_switchTilemap.enabled = false;
            //base_switchTilemap.SetActive(true);
        }

        Debug.Log("コライダーのオン、オフを切り替えました");
    }
}
