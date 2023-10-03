using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInspector : MonoBehaviour
{
    [SerializeField] private List<IShootable> shootList = new();  //インターフェースはSerializeField属性をつけても、public修飾子で宣言しても、デバッグモードでもインスペクターに表示されない。

    [SerializeField] private List<BulletBase> bulletList = new();  //こっちはクラスなので、インスペクターに表示される。
}
