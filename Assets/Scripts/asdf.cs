using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class asdf : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var tm = GetComponent<Tilemap>();
        var worldCorner = tm.GetComponent<TilemapRenderer>().bounds.min;
        Debug.Log("Cell(0,0) world corner = " + worldCorner);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
