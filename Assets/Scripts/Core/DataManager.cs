using System.IO;
using System.Net;
using Newtonsoft.Json;
using UnityEngine;

public class DataManager
{
    private static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new DataManager();

            return _instance;
        }
    }

    private string SavePath(string fileName)
    {
        string directoryPath = Path.Combine(Application.dataPath, "Resources/Datas");

        // ������ ������ ����
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        return Path.Combine(directoryPath, fileName + ".json");
    }

    public void SaveData<T>(T data, string fileName)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(SavePath(fileName), json);

        // ���� �� ��, �����Ϳ����� ���� ���ΰ�ħ �ʿ�
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public T LoadData<T>(string fileName)
    {
        // Resources/Datas ��ο��� �ҷ����� 
        string resourcePath = Path.Combine("Datas", fileName);
        TextAsset textAsset = Resources.Load<TextAsset>(resourcePath.Replace("\\", "/"));

        if (textAsset == null)
        {
            Debug.LogWarning($"�ε� ����: Resources/Datas/{fileName}.json ������ �����ϴ�.");
            return default(T);
        }

        return JsonConvert.DeserializeObject<T>(textAsset.text);


    }
}