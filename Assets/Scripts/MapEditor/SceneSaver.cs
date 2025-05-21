using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
        // (a) TilemapData 생성 로직 추가 가능
        var saveables = FindObjectsOfType<Saveable>();
        var objects = new List<ObjectData>();

        // id용 int 변수
        int i = 1;

        foreach (var s in saveables)
        { 
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

            // ConnectionEntry → ConnectionData
            foreach (var e in s.connections)
                if (e.target != null && e.action != null)
                    od.connections.Add(new ConnectionData
                    {
                        sourceId = s.id,
                        targetId = e.target.id,
                        actionType = e.action.name
                    });

            objects.Add(od);
            i++;
        }

        return new MapData { mapName = mapName, objects = objects };
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


