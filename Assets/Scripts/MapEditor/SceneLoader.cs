using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System;

public class SceneLoader : MonoBehaviour
{
    const string MAPS_FILE = "MapsData";

    public InteractionAction[] allActions;
    private Dictionary<string, InteractionAction> actionDict;

    void Awake()
    {
        actionDict = allActions.ToDictionary(a => a.name, a => a);
    }

    public void LoadMap(string mapName)
    {
        CleanScene();

        var db = DataManager.Instance.LoadData<MapDataBase>(MAPS_FILE) ?? new MapDataBase();
        var map = db.maps.Find(m => m.mapName == mapName);

        if (map == null)
        {
            Debug.LogWarning($"¸Ê '{mapName}' ¾øÀ½");
            return;
        }

        var gridGO  = new GameObject("Grid");
        gridGO.transform.localPosition = Vector3.zero;
        var grid    = gridGO.AddComponent<Grid>();

        foreach (var layer in map.tileMapLayers)
        {
            var layerGO = new GameObject(layer.layerName);
            layerGO.transform.SetParent(gridGO.transform, false);

            layerGO.transform.localPosition = new Vector3(layer.originX, layer.originY, 0);

            var tm = layerGO.AddComponent<Tilemap>();
            var tmRenderer = layerGO.AddComponent<TilemapRenderer>();
            layerGO.AddComponent<Rigidbody2D>().isKinematic = true;
            layerGO.AddComponent<TilemapCollider2D>();
            layerGO.transform.tag = "Ground";

            //Vector3 curCorner = tmRenderer.bounds.min;
            //layerGO.transform.position += curCorner;

            Debug.Log(tmRenderer.bounds.min);

            for(int i = 0; i < layer.tiles.Count; i++)
            {
                var td = layer.tiles[i];
                var tile = Resources.Load<Tile>("Tiles/" + td.tileName);
                tm.SetTile(new Vector3Int(td.x, td.y, 0), tile);
            }
        }

        var spawned = new Dictionary<string, GameObject>();
        foreach (var od in map.objects)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/" + od.prefabName);
            if (prefab == null) continue;

            var go = Instantiate(prefab);
            if(go == null)
            {
                Debug.LogWarning($"LoadMap: {prefab} ¾øÀ½");
                continue;
            }

            go.name = od.instanceName;

            go.transform.position    = new Vector3(od.posX, od.posY, od.posZ);
            go.transform.eulerAngles = new Vector3(od.rotX, od.rotY, od.rotZ);
            go.transform.localScale  = new Vector3(od.scaleX, od.scaleY, od.scaleZ);
            go.SetActive(od.isActive);

            var s = go.GetComponent<Saveable>();
            s.id = od.id;

            spawned[od.id] = go;
        }

        foreach (var od in map.objects)
        {
            var s = spawned[od.id].GetComponent<Saveable>();
            s.connections.Clear();

            if (od.connections != null)
            {
                foreach (var cd in od.connections)
                {
                    if (!spawned.ContainsKey(cd.targetId) || !actionDict.ContainsKey(cd.actionType))
                        continue;

                    s.connections.Add(new ConnectionEntry
                    {
                        target = spawned[cd.targetId].GetComponent<Saveable>(),
                        action = actionDict[cd.actionType]
                    });
                } 
            }
        }

        Debug.Log($"¸Ê '{mapName}' ·Îµå ¿Ï·á");
    }

    private void CleanScene()
    {
        foreach (var s in FindObjectsOfType<Saveable>(includeInactive: true))
            Destroy(s.gameObject);

        foreach (var grid in FindObjectsOfType<Grid>(includeInactive: true))
            Destroy(grid.gameObject);
    }
}