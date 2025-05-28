using System;
using System.Collections.Generic;


[Serializable]
public class ObjectData
{
    public string id; 
    public string prefabName;   // Resources에서 로드할 prefab 이름 
    public string instanceName; // Scene에 생성될 object 이름

    // Vector3는 내부에 있는 normalized 같은 프로퍼티가 담겨있는 복잡한 타입
    // 이런 Vector3를 그대로 직렬화 하려하면 무한 참조에 빠짐
    // 따라서 Vector3를 그대로 Newtonsoft.Json 에 넣으면 안됨
    // 그래서 이런식으로 X, Y, Z 값을 분해해서 넣을 필요가 있음
    public float posX, posY, posZ;
    public float rotX, rotY, rotZ;
    public float scaleX, scaleY, scaleZ;

    public List<ConnectionData> connections;

    public bool isActive;
}

[Serializable]
public class ConnectionData
{
    public string sourceId;     // 이 오브젝트의 고유 ID
    public string targetId;     // 연결될 오브젝트의 ID
    public string actionType;   // 실행할 event
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