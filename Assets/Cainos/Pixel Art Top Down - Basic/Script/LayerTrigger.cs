﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    //when object exit the trigger, put it to the assigned layer and sorting layers
    //used in the stair objects for player to travel between layers
    public class LayerTrigger : MonoBehaviour
    {
        //public string layer;
        //public string sortingLayer;

        //[SerializeField] private MapManager mapManager;


        /// <summary>
        /// 上記メソッドで記録しておいた座標との大小に応じてisClimbStairsの真偽を決定する
        /// </summary>
        /// <param name="other"></param>
        //private void OnTriggerExit2D(Collider2D other)
        //{
            //同じようなことを自分で制御してあるのでコメントアウト
            //other.gameObject.layer = LayerMask.NameToLayer(layer);

            //other.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayer;
            //SpriteRenderer[] srs = other.gameObject.transform.GetChild(0).gameObject.GetComponentsInChildren<SpriteRenderer>();
            //foreach (SpriteRenderer sr in srs)
            //{
            //    sr.sortingLayerName = sortingLayer;
            //}

            //if (other.CompareTag("Player"))
            //{
            //    if (!mapManager.isClimbedStairs)
            //    {
            //        mapManager.isClimbedStairs = true;

            //        mapManager.JudgeClimbedStairs();

            //        return;
            //    }
            //    else
            //    {
            //        mapManager.isClimbedStairs = false;

            //        mapManager.JudgeClimbedStairs();

            //        return;
            //    }
            //}
        //}
    }
}
