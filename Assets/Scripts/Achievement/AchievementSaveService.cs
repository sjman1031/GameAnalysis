using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AchievementSaveService
{
    private const string FILE_NAME = "Achievements";

    public static void Save(AchievementDataBase db)
    {
        DataManager.Instance.SaveData(db, FILE_NAME);
    }

    public static AchievementDataBase Load()
    {
        var loaded = DataManager.Instance.LoadData<AchievementDataBase>(FILE_NAME);

        if (loaded != null)
        {
            var newDB = new AchievementDataBase();
            DataManager.Instance.SaveData(newDB, FILE_NAME);
            return newDB;
        }

        return loaded;
    }

    public static void Delete()
    {
        string path = Path.Combine(Application.persistentDataPath, FILE_NAME + ".json");
        if(File.Exists(path)) File.Delete(path);
    }
}