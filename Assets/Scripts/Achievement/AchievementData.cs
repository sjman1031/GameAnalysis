using UnityEngine;
using System.Collections.Generic;

public enum AchievementConditionType
{
    None,
    Stage_Clear,
    Hidden_Stage_Clear,
    Bonus_Collectibles,
    Game_Play,
    Achievement_Clear,
}


[CreateAssetMenu(fileName = "Achievement", menuName = "Game/Achievement", order = 1)]
public class AchievementData : ScriptableObject
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

[CreateAssetMenu(fileName = "AchievementDataBase", menuName = "Game/AchievementDatabase", order = 2), System.Serializable]
public class AchievementDataBase :ScriptableObject
{
    public List<AchievementData> achievements = new();
}