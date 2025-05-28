using System;
using System.Collections.Generic;


[Serializable]
public class ObjectData
{
    public string id; 
    public string prefabName;   // Resources���� �ε��� prefab �̸� 
    public string instanceName; // Scene�� ������ object �̸�

    // Vector3�� ���ο� �ִ� normalized ���� ������Ƽ�� ����ִ� ������ Ÿ��
    // �̷� Vector3�� �״�� ����ȭ �Ϸ��ϸ� ���� ������ ����
    // ���� Vector3�� �״�� Newtonsoft.Json �� ������ �ȵ�
    // �׷��� �̷������� X, Y, Z ���� �����ؼ� ���� �ʿ䰡 ����
    public float posX, posY, posZ;
    public float rotX, rotY, rotZ;
    public float scaleX, scaleY, scaleZ;

    public List<ConnectionData> connections;

    public bool isActive;
}

[Serializable]
public class ConnectionData
{
    public string sourceId;     // �� ������Ʈ�� ���� ID
    public string targetId;     // ����� ������Ʈ�� ID
    public string actionType;   // ������ event
}

[Serializable]
public class MapData
{
    public string mapName;
    public List<ObjectData> objects;

    public List<TileMapLayerData> tileMapLayers = new List<TileMapLayerData>();
}

[Serializable]
public class MapDataBase
{
    public List<MapData> maps = new List<MapData>();
}