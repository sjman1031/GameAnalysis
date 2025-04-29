using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectData
{
    public string prefabName;   // Resources���� �ε��� prefab �̸� 
    public string instanceName; // Scene�� ������ object �̸�

    // Vector3�� ���ο� �ִ� normalized ���� ������Ƽ�� ����ִ� ������ Ÿ��
    // �̷� Vector3�� �״�� ����ȭ �Ϸ��ϸ� ���� ������ ����
    // ���� Vector3�� �״�� Newtonsoft.Json �� ������ �ȵ�
    // �׷��� �̷������� X, Y, Z ���� �����ؼ� ���� �ʿ䰡 ����
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
    [Header("������ Object �̸���")]
    public List<string> excludeNames = new List<string> { "SceneSaver", "Main Camera", "SaveButton", "LoadButton" };

    public void SaveSceneObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        List<ObjectData> objectDatas = new List<ObjectData>();

        foreach(var obj in allObjects)
        {
            // ������ object ����
            if (obj.hideFlags == HideFlags.NotEditable || obj.hideFlags == HideFlags.HideAndDontSave)
                continue;
            if(!obj.activeInHierarchy)
                continue;
            if (excludeNames.Contains(obj.name))
                continue;

            string prefabName = obj.name; // (�ӽ�) prefab name = object name

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
        Debug.Log($"Scene Object ���� �Ϸ�! ����� Object ��: {objectDatas.Count}");
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

