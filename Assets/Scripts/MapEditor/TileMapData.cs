using System;
using System.Collections.Generic;

[Serializable]
public class TileData
{
    public int x, y;
    public string tileName;
}

[Serializable]
public class TileMapLayerData
{
    public string layerName;
    public float originX;
    public float originY;
    public int width, height;
    public List<TileData> tiles = new List<TileData>();
}

[Serializable]
public class TilemapData
{
    public List<TileMapLayerData> layers = new();
}