using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;

public class SceneSaver : MonoBehaviour
{
    const string MAPS_FILE = "MapsData";

    public void SaveMap(string mapName)
    {
        var newMap = CreateMapData(mapName);
        var db = LoadAllMaps();

        db.maps.RemoveAll(m => m.mapName == mapName);
        db.maps.Add(newMap);

        DataManager.Instance.SaveData(db, MAPS_FILE);

        Debug.Log($"�� '{mapName}' ���� �Ϸ� �� Resources/Datas/{MAPS_FILE}.json");
    }

    MapData CreateMapData(string mapName)
    {
        var mapData = new MapData
        {
            mapName = mapName, 
            tileMapLayers = new List<TileMapLayerData>(),
            objects = new List<ObjectData>()
        };

        var allTileMap = FindObjectsOfType<Tilemap>();
        foreach (var tm in allTileMap)
        {
            var bounds = tm.cellBounds;
            var layer = new TileMapLayerData
            {
                layerName       = tm.gameObject.name,
                originX         = bounds.xMin,
                originY         = bounds.yMin,
                width           = bounds.size.x,
                height          = bounds.size.y,
                tiles           = new List<TileData>()
            };


            for (int x = bounds.xMin; x < bounds.xMax; x++)
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    var tile = tm.GetTile(new Vector3Int(x, y, 0));
                    if (tile == null)
                    {
                        layer.tiles.Add(new TileData
                        {
                            x = x - bounds.xMin,
                            y = y - bounds.yMin,
                            tileName = "null"
                        });
                    }

                    else 
                    { 
                        layer.tiles.Add(new TileData
                        {
                            x = x - bounds.xMin,
                            y = y - bounds.yMin,
                            tileName = tile.name
                        });
                    }
                }

            mapData.tileMapLayers.Add(layer);
        }

        // id�� int ����
        int i = 1;
        var saveables = FindObjectsOfType<Saveable>();

        foreach (var s in saveables)
        { 
            string newID = i.ToString();
            s.id = newID;

            var od = new ObjectData
            {
                id = i.ToString(),
                prefabName = s.prefabName,
                instanceName = s.gameObject.name,

                posX = s.transform.position.x,
                posY = s.transform.position.y,
                posZ = s.transform.position.z,

                rotX = s.transform.eulerAngles.x,
                rotY = s.transform.eulerAngles.y,
                rotZ = s.transform.eulerAngles.z,

                scaleX = s.transform.localScale.x,
                scaleY = s.transform.localScale.y,
                scaleZ = s.transform.localScale.z,

                isActive = s.gameObject.activeSelf
            };

            // ConnectionEntry �� ConnectionData
            foreach (var e in s.connections)
                if (e.target != null && e.action != null)
                    od.connections.Add(new ConnectionData
                    {
                        sourceId = s.id,
                        targetId = e.target.id,
                        actionType = e.action.name
                    });

            mapData.objects.Add(od);
            i++;
        }

        return mapData;
    }
    public void DeleteMap(string mapName)
    {
        var db = LoadAllMaps();
        int removed = db.maps.RemoveAll(m => m.mapName == mapName);
        if (removed > 0)
        {
            DataManager.Instance.SaveData(db, MAPS_FILE);
            Debug.Log($"�� '{mapName}' ���� �Ϸ�.");
        }
        else
        {
            Debug.LogWarning($"���� ����: �� '{mapName}'��(��) ã�� �� �����ϴ�.");
        }
    }

    public List<String> GetMapNames()
    {
        return LoadAllMaps().maps.Select(m => m.mapName).ToList();
    }

    MapDataBase LoadAllMaps()
    {
        var db = DataManager.Instance.LoadData<MapDataBase>(MAPS_FILE);
        return db ?? new MapDataBase();
    }
}


