using UnityEngine;

/// <summary>
/// キャラの管理クラス
/// </summary>
public class CharaManager : MonoBehaviour
{
    private CharaMove charaMove;
    private CharaAnime charaAnime;
    private CharaAttack charaAttack;

    private Vector2 direction;
    public Vector2 Direction => direction;


    //void Start()
    //{
    //    //デバッグ用
    //    SetUpCharaManager();
    //}

    void Update()
    {
        if (charaMove == null || charaAnime == null)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 tapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            charaMove.Move(tapPos);

            //向きの更新
            UpdateDirection(tapPos);

            //移動の向きとアニメの向きの同期
            charaAnime.UpdateAnimation(direction);
        }

        //CharaAttackに攻撃用の向きの情報を提供
        if (charaAttack != null)
        {
            charaAttack.UpdateAttackDirection(direction);
        }

        //攻撃の一時停止と再開
        //if (Input.GetMouseButtonDown(1))
        //{
        //    charaAttack.ToggleAttack();
        //}
    }

    /// <summary>
    /// 設定
    /// </summary>
    public void SetUpCharaManager()
    {
        IChara[] charas = GetComponents<IChara>();

        //各子クラスの初期設定(インターフェースで取得してあるので、子クラスが違ってもforeachでまとめて処理できる)
        foreach (var chara in charas)
        {
            chara.SetUpChara();
        }

        //各子クラスの取得
        TryGetComponent(out charaMove);
        TryGetComponent(out charaAnime);

        if (TryGetComponent(out charaAttack))
        {
            charaAttack.ExecutePrepareAttack();
        }
    }

    /// <summary>
    /// 向きの更新
    /// </summary>
    /// <param name="newPos"></param>
    private void UpdateDirection(Vector2 newPos)
    {
        direction = (newPos - (Vector2)transform.position).normalized;
    }
}
