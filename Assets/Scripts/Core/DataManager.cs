using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class DataManager
{
    private static DataManager _instance;
    
    private DataManager() { }

    public static DataManager Instance
    {  
        get 
        {
            if(_instance == null)
               _instance = new DataManager();
            return _instance; 
        } 
    }

    public void LoadDatas(string dataPath)
    {
        var json = Resources.Load<TextAsset>(dataPath).text;
    }
}
