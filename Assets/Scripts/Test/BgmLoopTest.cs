using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//この検証から、「BGMが鳴ったままゲームが動かなくなってしまう」原因がwhile文にあることを証明できる。
public class BgmLoopTest : MonoBehaviour
{
    public AudioSource testSource;


    //IEnumerator Start()
    //{
    //    yield return new WaitForSeconds(2f);

    //    while (true)
    //    {
    //        Debug.Log("ループ中");
    //    }
    //}

    void Start()
    {
        //先にBGMを鳴らす
        testSource.Play();

        while (true)
        {
            Debug.Log("ループ中");
        }
    }
}
