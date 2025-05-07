using System.Collections.Generic;
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
}