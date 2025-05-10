using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
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

// ���� ������ ������Ʈ���� ���� ������Ʈ
public class Saveable :MonoBehaviour
{
    public string prefabName;
}
