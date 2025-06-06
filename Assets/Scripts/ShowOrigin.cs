using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShowOrigin : MonoBehaviour
{

    void Start()
    {
        TilemapRenderer tmRenderer = gameObject.GetComponentInChildren<TilemapRenderer>();

        Debug.Log(tmRenderer.bounds.min);
    }
}
