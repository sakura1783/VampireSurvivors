using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シューティングゲームやアクションゲームなどで使用される弾の発射操作を抽象化するインターフェース。
/// IShootableを実装するクラスは、共通の弾の発射操作を提供し、ゲーム内の様々な要素が一貫した方法で弾を発射できるようにする。
/// </summary>
public interface IShootable
{
    void Shoot(Vector2 direction);  //インターフェースではアクセス修飾子を省略した場合、自動的にpublic扱いになる
}
