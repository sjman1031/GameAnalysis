using System;
using System.Collections.Generic;

[Serializable]
public class TileData
{
    public int x, y;
    public string tileId;
}

[Serializable]
public class TileMapLayerData
{
    public string layerName;
    public int width, height;
    public List<TileData> tiles;
}

[Serializable]
public class TilemapData
{
    public List<TileMapLayerData> layers = new();
}