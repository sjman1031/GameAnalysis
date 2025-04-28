using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SaveData()
    {

    }


    public void LoadData(string dataPath)
    {
        var json = Resources.Load<TextAsset>(dataPath).text;
    }
}
