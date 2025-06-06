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

        Debug.Log($"맵 '{mapName}' 저장 완료 → Resources/Datas/{MAPS_FILE}.json");
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
            var renderer = tm.GetComponent<TilemapRenderer>();
            var layer = new TileMapLayerData
            {
                layerName       = tm.gameObject.name,
                originX         = renderer.bounds.min.x,
                originY         = renderer.bounds.min.y,
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

        var saveables = FindObjectsOfType<Saveable>(includeInactive: true).OrderBy(s => s.name).ToArray();
        var lookup = new Dictionary<Saveable, ObjectData>();

        for(int i = 0; i < saveables.Length; i++)
        {
            var s = saveables[i];
            s.id = (i + 1).ToString();

            var od = new ObjectData
            {
                id              = s.id,
                prefabName      = s.prefabName,
                instanceName    = s.gameObject.name,
                posX            = s.transform.position.x,
                posY            = s.transform.position.y,
                posZ            = s.transform.position.z,
                rotX            = s.transform.eulerAngles.x,
                rotY            = s.transform.eulerAngles.y,
                rotZ            = s.transform.eulerAngles.z,
                scaleX          = s.transform.localScale.x,
                scaleY          = s.transform.localScale.y,
                scaleZ          = s.transform.localScale.z,
                isActive        = s.gameObject.activeSelf,
                connections     = new List<ConnectionData>()
            };

            lookup[s] = od;
            mapData.objects.Add(od);    
        }

        foreach(var s in saveables)
        {
            var od = lookup[s];
            
            foreach(var e in s.connections)
            {
                if (e.target != null && e.action != null)
                {
                    var targetOD = lookup[e.target];
                    od.connections.Add(new ConnectionData
                    {
                        sourceId = od.id,
                        targetId = targetOD.id,
                        actionType = e.action.name
                    });
                }
                else
                    Debug.Log($"[{s.name}] 유효하지 않은 connection entry: {e}");
            }
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
            Debug.Log($"맵 '{mapName}' 삭제 완료.");
        }
        else
        {
            Debug.LogWarning($"삭제 실패: 맵 '{mapName}'을(를) 찾을 수 없습니다.");
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


