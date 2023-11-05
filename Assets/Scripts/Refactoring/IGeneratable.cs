using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGeneratable
{
    //void GenerateBullet(Vector2 direction);

    /// <summary>
    /// 上記メソッドのオーバーロード(名前は同じだが引数が異なる)。第2引数で任意の型を受け取れるようにして汎用性を高めている
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="direction"></param>
    /// <param name="t"></param>
    void GenerateBullet<T>(Vector2 direction, T t);
}
