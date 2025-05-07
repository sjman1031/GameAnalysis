using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class AchievementEditorUI : MonoBehaviour
{
    public TMP_Dropdown achievementDropdown;
    public TMP_Dropdown conditionDropdown;
    public TMP_InputField titleInput, descInput, idInput, countInput;
    public Button saveButton, loadButton;
    private AchievementDataBase db;

    private void Start()
    {
        db = AchievementSaveService.Load();
        PopulateConditionDropdown();
        RefreshUI();

        achievementDropdown.onValueChanged.AddListener(UpdateFields);
        saveButton.onClick.AddListener(SaveJSON);
        loadButton.onClick.AddListener(LoadJSON);
    }

    void PopulateConditionDropdown()
    {
        conditionDropdown.ClearOptions();
        conditionDropdown.AddOptions(System.Enum.GetNames(typeof(AchievementConditionType)).ToList());
    }

    void RefreshUI()
    {
        achievementDropdown.ClearOptions();

        var options = new List<string> { "Add New Achievement" };
        options.AddRange(db.achievements.Select(a => a.title));
        achievementDropdown.AddOptions(options);

        achievementDropdown.value = 0;
        achievementDropdown.RefreshShownValue();
        UpdateFields(0);
    }

    void UpdateFields(int index)
    {
        var a = db.achievements[index];
        titleInput.text = a.title;
        descInput.text = a.description;
        idInput.text = a.id;
        countInput.text = a.requiredCount.ToString();
        conditionDropdown.value = (int)a.conditionType;
    }

    void SaveJSON()
    {
        int index = achievementDropdown.value;

        if (index == 0)
        { 
            AddNewAchievementFromFields();
            Debug.Log("�� ������ �߰� �Ǿ����ϴ�.");
        }
        else
        {
            ApplyFieldsToData(index - 1); // ���� ����Ʈ�� 0���� ���� ������ -1
            Debug.Log("������ �����Ǿ����ϴ�.");
        }

        AchievementSaveService.Save(db);
        RefreshUI();
        Debug.Log("���� ���� �Ϸ�");
    }

    void LoadJSON()
    {
        db = AchievementSaveService.Load();
        RefreshUI();
        Debug.Log("���� �ҷ����� �Ϸ�");
    }

    void ApplyFieldsToData(int index)
    {
        var a = db.achievements[index];
        a.title = titleInput.text;
        a.description = descInput.text;
        a.id = idInput.text;
        a.requiredCount = int.TryParse(countInput.text, out int v) ? v : 1;
        a.conditionType = (AchievementConditionType)conditionDropdown.value;
    }

    void AddNewAchievementFromFields()
    {
        AchievementData newAhievement = new AchievementData
        {
            title = titleInput.text,
            description = descInput.text,
            id = idInput.text,
            requiredCount = int.TryParse(countInput.text, out int v) ? v : 1,
            conditionType = (AchievementConditionType)conditionDropdown.value,
            currentCount = 0,
            isUnlocked = false
        };

        db.achievements.Add(newAhievement);
    }
}
