using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementManager : MonoBehaviour
{
    [Header("업적 데이터베이스")]
    public AchievementDataBase achievementdata;

    public void AddProgress(string achievementID, int value = 1)
    {
        var achievement = achievementdata.achievements.Find(a => a.id == achievementID);
        if (achievement == null) return;
        if (achievement.isUnlocked) return;

        achievement.currentCount += value;

        if(achievement.currentCount >= achievement.requiredCount)
        {
            achievement.isUnlocked = true;
            Debug.Log($"업적 달성: {achievement.title}");

            // TODO 
            // InGame 연출
        }
    }

    public void ResetAllAchievements()
    {
        foreach(var a in achievementdata.achievements)
        {
            a.currentCount = 0;
            a.isUnlocked= false;
        }
    }

    public void ReportCondition(AchievementConditionType condition, int value = 1)
    {
        foreach (var a in achievementdata.achievements)
        {
            if (a.isUnlocked) continue;
            if (a.conditionType != condition) continue;
            a.currentCount += value;

            if (a.currentCount >= a.requiredCount)
            {
                a.isUnlocked = true;
                Debug.Log($"업적 달성: {a.title}");

                // TODO 
                // InGame 연출
            }
        }
    }
}
