using UnityEngine;
using System.Collections.Generic;
using System;

public enum AchievementConditionType
{
    None,
    Stage_Clear,
    Hidden_Stage_Clear,
    Bonus_Collectibles,
    Game_Play,
    Achievement_Clear,
}

public class AchievementData
{
    public string id;                               // 고유 ID
    public string title;                            // 업적 이름
    public string description;                      // 설명
    public Sprite icon;                             // 업적 아이콘
    public AchievementConditionType conditionType;  // 업적 타입
    public int requiredCount;                       // 달성 조건 수치

    [HideInInspector]
    public int currentCount;                        // 현재 달성 수치
    [HideInInspector]       
    public bool isUnlocked;                         // 달성했으면 true, 아니면 false
}

[Serializable]
public class AchievementDataBase
{
    public List<AchievementData> achievements = new();
}