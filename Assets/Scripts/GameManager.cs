using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapClearData
{
    public int triedCount = 0;
    public float clearTime = 0f;
}


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                Debug.LogError("GameManager �ν��Ͻ��� �������� �ʽ��ϴ�.");
            }

            return _instance;
        }
    }


    public Dictionary<string, List<MapClearData>> mapClearData = new Dictionary<string, List<MapClearData>>();



    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(_instance != this)
            Destroy(gameObject);    
    }

    public void SaveStageClear(string stageName, float clearTime)
    {
        if(!mapClearData.ContainsKey(stageName))
            mapClearData[stageName] = new List<MapClearData>();

        MapClearData data;
        if (mapClearData[stageName].Count > 0)
            data = mapClearData[stageName][mapClearData[stageName].Count - 1];
        else
        {
            data = new MapClearData();
            mapClearData[stageName].Add(data);
        }

        data.triedCount += 1;
        data.clearTime= clearTime;

        DataManager.Instance.SaveData(mapClearData, "StageClearData.json");
    }
}
