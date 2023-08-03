using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    //自分で制御してあるのでコメントアウト

    //when object exit the trigger, put it to the assigned layer and sorting layers
    //used in the stair objects for player to travel between layers
    public class LayerTrigger : MonoBehaviour
    {
        //public string layer;
        //public string sortingLayer;

        //private void OnTriggerExit2D(Collider2D other)
        //{
        //    other.gameObject.layer = LayerMask.NameToLayer(layer);

        //    other.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayer;
        //    SpriteRenderer[] srs = other.gameObject.transform.GetChild(0).gameObject.GetComponentsInChildren<SpriteRenderer>();
        //    foreach ( SpriteRenderer sr in srs)
        //    {
        //        sr.sortingLayerName = sortingLayer;
        //    }
        //}

    }
}
