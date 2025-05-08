using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementManager : MonoBehaviour
{
    [Header("업적 데이터베이스")]
    public AchievementDataBase achievementdata;

    private void Awake()
    {
        achievementdata = AchievementSaveService.Load();
    }

    public void ReportCondition(AchievementConditionType condition, int value = 1)
    {
        foreach (var a in achievementdata.achievements)
        {
            if (a.isUnlocked || a.conditionType != condition) continue;

            a.currentCount += value;
            if(a.currentCount >= a.requiredCount)
            {
                a.isUnlocked = true;
                Debug.Log($"업적 달성: {a.title}");
            }
        }
    }

    public void SaveAchievements()
    {
        AchievementSaveService.Save(achievementdata);
    }

    public void ResetAll()
    {
        foreach(var a in achievementdata.achievements)
        {
            a.currentCount = 0;
            a.isUnlocked = false;
        }
    }
}
