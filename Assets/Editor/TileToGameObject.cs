using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileToGameObject : MonoBehaviour
{
    [MenuItem("Tools/Convert Tilemap -> GameObjects")]
    public static void Convert()
    {
        var tmGO = Selection.activeGameObject;
        var tm = tmGO?.GetComponent<Tilemap>();

        if(tm == null)
        {
            Debug.LogWarning("Tilemap이 붙어있는 GameObject를 선택하세요.");
            return;
        }

        foreach(var cell in tm.cellBounds.allPositionsWithin)
        {
            if (!tm.HasTile(cell))
                continue;

            var sprite = tm.GetSprite(cell);
            if(sprite == null)
                continue;

            var go = new GameObject($"Tile_{cell.x}_{cell.y}");
            go.transform.position = tm.CellToWorld(cell) + tm.tileAnchor;

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;

            var rend = tm.GetComponent<TilemapRenderer>();
            if(rend != null)
            {
                sr.sortingLayerID = rend.sortingLayerID;
                sr.sortingOrder = rend.sortingOrder;
            }
        }

        tmGO.SetActive(false);
        Debug.Log("변환 완료");
    }
}
