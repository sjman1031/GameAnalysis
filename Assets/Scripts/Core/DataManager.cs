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

        // 폴더가 없으면 생성
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        return Path.Combine(directoryPath, fileName + ".json");
    }

    public void SaveData<T>(T data, string fileName)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(SavePath(fileName), json);

        // 저장 한 후, 에디터에서는 파일 새로고침 필요
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public T LoadData<T>(string fileName)
    {
        // Resources/Datas 경로에서 불러오기 
        string resourcePath = Path.Combine("Datas", fileName);
        TextAsset textAsset = Resources.Load<TextAsset>(resourcePath.Replace("\\", "/"));

        if (textAsset == null)
        {
            Debug.LogWarning($"로드 실패: Resources/Datas/{fileName}.json 파일이 없습니다.");
            return default(T);
        }

        return JsonConvert.DeserializeObject<T>(textAsset.text);


    }
}