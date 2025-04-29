using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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


public class SceneSaver : MonoBehaviour
{
    [Header("제외할 Object 이름들")]
    public List<string> excludeNames = new List<string> { "SceneSaver", "Main Camera", "SaveButton", "LoadButton" };

    public void SaveSceneObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        List<ObjectData> objectDatas = new List<ObjectData>();

        foreach(var obj in allObjects)
        {
            // 제외할 object 선별
            if (obj.hideFlags == HideFlags.NotEditable || obj.hideFlags == HideFlags.HideAndDontSave)
                continue;
            if(!obj.activeInHierarchy)
                continue;
            if (excludeNames.Contains(obj.name))
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
                scaleZ = obj.transform.localScale.z
            };
            objectDatas.Add(data);  
        }

        DataManager.Instance.SaveData(objectDatas, "sceneData");
        Debug.Log($"Scene Object 저장 완료! 저장된 Object 수: {objectDatas.Count}");
    }
}

[Serializable]
public class Vector3Serializable
{
    float x, y, z;

    public Vector3Serializable(Vector3 vector)
    {
        x = vector.x; 
        y =vector.y; 
        z =vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x,y,z);
    }
}

