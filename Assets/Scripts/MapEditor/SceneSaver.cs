using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class ObjectData
{
    public string prefabName;   // Resources에서 로드할 prefab 이름 
    public string instanceName; // Scene에 생성될 object 이름

    // Vector3는 내부에 있는 normalized 같은 프로퍼티가 담겨있는 복잡한 타입
    // 이런 Vector3를 그대로 직렬화 하려하면 무한 참조에 빠짐
    // 따라서 Vector3를 그대로 Newtonsoft.Json 에 넣으면 안됨
    // 그래서 이런식으로 X, Y, Z 값을 분해해서 넣을 필요가 있음
    public float posX;
    public float posY;
    public float posZ;

    public float rotX;
    public float rotY;
    public float rotZ;

    public float scaleX;
    public float scaleY;
    public float scaleZ;
}

[Serializable]
public class MapData
{
    public string mapName;
    public List<ObjectData> objects;
}

[Serializable]
public class MapDataBase
{
    public List<MapData> maps = new List<MapData>();
}

public class SceneSaver : MonoBehaviour
{
    private const string MAPS_FILE = "MapsData";

    [Header("제외할 Object 이름들")]
    public List<string> excludeNames = new List<string> { "SceneSaver", "Main Camera", "SaveButton", "LoadButton" };

    // Scene을 MapData로 변환
    private MapData CreateMapData(string mapName)
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<ObjectData> objects = new List<ObjectData>();

        foreach(var obj in allObjects)
        {
            // 제외할 object 선별
            if (!ShouldSaveObject(obj))
                continue;

            string prefabName = obj.name; // (임시) prefab name = object name

            ObjectData data = new ObjectData()
            {
                prefabName      = prefabName,
                instanceName    = obj.name,

                posX = obj.transform.position.x,
                posY = obj.transform.position.y,
                posZ = obj.transform.position.z,

                rotX = obj.transform.eulerAngles.x,
                rotY = obj.transform.eulerAngles.y,
                rotZ = obj.transform.eulerAngles.z,

                scaleX = obj.transform.localScale.x,
                scaleY = obj.transform.localScale.y,
                scaleZ = obj.transform.localScale.z,
            };

            objects.Add(data);  
        }

        return new MapData { mapName = mapName, objects = objects };
    }

    public void SaveMap(string mapName)
    {
        MapData newMap = CreateMapData(mapName);
        MapDataBase db = LoadAllMaps();

        // 중복 map 제거 후 추가
        db.maps.RemoveAll(m => m.mapName == mapName);
        db.maps.Add(newMap);

        DataManager.Instance.SaveData(db, MAPS_FILE);
        Debug.Log($"'{mapName}' 저장 완료");
    }

    public void LoadMap(string mapName)
    {
        // Scene Clear
        ClearScene();

        MapDataBase db = LoadAllMaps();
        MapData map = db.maps.FirstOrDefault(m  => m.mapName == mapName);
        
        if(map == null)
        {
            Debug.LogWarning($"'{mapName}' 맵을 찾을 수 없습니다.");
            return;
        }

        foreach (var data in map.objects)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/" + data.prefabName);
            if (prefab != null)
            {
                GameObject obj = GameObject.Instantiate(prefab);
                obj.name = data.instanceName;
                obj.transform.position      = new Vector3(data.posX, data.posY, data.posZ);
                obj.transform.eulerAngles   = new Vector3(data.rotX, data.rotY, data.rotZ);
                obj.transform.localScale    = new Vector3(data.scaleX, data.scaleY, data.scaleZ);
            }
        }

        Debug.Log($"'{mapName}' 로드 완료");
    }

    public void DeleteMap(string mapName)
    {
        MapDataBase db = LoadAllMaps();
        if (db.maps.RemoveAll(m => m.mapName == mapName) > 0)
        {
            DataManager.Instance.SaveData(db, MAPS_FILE);
            Debug.Log($"'{mapName}' 삭제 완료");
        }
        else
            Debug.LogWarning($"'{mapName}' 삭제 실패 (없음)");
    }

    public bool IsMapExists(string mapName)
    {
        return LoadAllMaps().maps.Any(m => m.mapName == mapName); 
    }

    public List<String> GetMapNames()
    {
        return LoadAllMaps().maps.Select(m => m.mapName).ToList();  
    }
    private MapDataBase LoadAllMaps()
    {
        return DataManager.Instance.LoadData<MapDataBase>(MAPS_FILE) ?? new MapDataBase();  
    }

    private void ClearScene()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach(var obj in allObjects)
        {
            if (!ShouldSaveObject(obj))
                continue;
            Destroy(obj);   
        }
    }

    private bool ShouldSaveObject(GameObject obj)
    {
        // 필수 조건
        if (obj.GetComponent<MapSaveable>() == null) return false;

        // 추가 조건
        if (obj.hideFlags == HideFlags.NotEditable || obj.hideFlags == HideFlags.HideAndDontSave) return false;
        if (!obj.activeInHierarchy) return false;
        if (excludeNames.Contains(obj.name)) return false;
        if (obj.scene.name != UnityEngine.SceneManagement.SceneManager.GetActiveScene().name) return false;

        return true;
    }
}


