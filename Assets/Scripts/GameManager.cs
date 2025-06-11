using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapClearData
{
    public int triedCount;
    public float clearTime;
    public bool isCleared = false;
}


public class GameManager : MonoBehaviourPun
{
    public Dictionary<string, List<MapClearData>> mapClearData
        = new Dictionary<string, List<MapClearData>>();

    private string currentMapName;
    private float mapStartTime;
    private Dictionary<string, int> tryCounts = new();


    public void OnMapLoaded(string mapName)
    {
        var history = GetClearHistory(mapName);
    }

    public void OnMapStart(string mapName)
    {
        currentMapName = mapName;
        if (!tryCounts.ContainsKey(mapName))
            tryCounts[mapName] = 0;
        tryCounts[mapName]++;

        mapStartTime = Time.time;
    }

    public void OnMapClear()
    {
        float clearTime = Time.time - mapStartTime;

        if (!mapClearData.ContainsKey(currentMapName))
            mapClearData[currentMapName] = new List<MapClearData>();

        var record = new MapClearData
        {
            triedCount = tryCounts[currentMapName],
            clearTime = clearTime,
            isCleared = true
        };

        mapClearData[currentMapName].Add(record);

        string fileName = $"Stage_{currentMapName}.json";
        DataManager.Instance.SaveData(mapClearData[currentMapName], fileName);

        Debug.Log($"�� '{currentMapName}' Ŭ����! �õ� {record.triedCount}ȸ, Ŭ���� Ÿ�� {record.clearTime:F2}s");
    }

    public List<MapClearData> GetClearHistory(string mapName)
    {
        if (mapClearData.TryGetValue(mapName, out var history))
            return history;

        return new List<MapClearData>();
    }
}
