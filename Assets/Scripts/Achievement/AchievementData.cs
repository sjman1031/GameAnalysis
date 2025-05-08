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
    public string id;                               // ���� ID
    public string title;                            // ���� �̸�
    public string description;                      // ����
    public Sprite icon;                             // ���� ������
    public AchievementConditionType conditionType;  // ���� Ÿ��
    public int requiredCount;                       // �޼� ���� ��ġ

    [HideInInspector]
    public int currentCount;                        // ���� �޼� ��ġ
    [HideInInspector]       
    public bool isUnlocked;                         // �޼������� true, �ƴϸ� false
}

[Serializable]
public class AchievementDataBase
{
    public List<AchievementData> achievements = new();
}