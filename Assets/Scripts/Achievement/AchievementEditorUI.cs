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

    private AchievementDataBase achievementdata;

    private void Start()
    {
        achievementdata = AchievementSaveService.Load();
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
        options.AddRange(achievementdata.achievements.Select(a => a.title));
        achievementDropdown.AddOptions(options);

        achievementDropdown.value = 0;
        achievementDropdown.RefreshShownValue();
        UpdateFields(0);
    }

    void UpdateFields(int index)
    {
        if (index <= 0 || index > achievementdata.achievements.Count) return;
        var a = achievementdata.achievements[index - 1];
        titleInput.text = a.title;
        descInput.text = a.description;
        idInput.text = a.id;
        countInput.text = a.requiredCount.ToString();
        conditionDropdown.value = (int)a.conditionType;
    }

    void ApplyFieldsToData(int index)
    {
        if (index < 0 || index >= achievementdata.achievements.Count) return;
        var a = achievementdata.achievements[index];
        a.title = titleInput.text;
        a.description = descInput.text;
        a.id = idInput.text;
        a.requiredCount = int.TryParse(countInput.text, out int v) ? v : 1;
        a.conditionType = (AchievementConditionType)conditionDropdown.value;
    }

    void AddNewAchievementFromFields()
    {
        if (IsDuplicateID(idInput.text.Trim()))
        {
            Debug.LogWarning("ID 중복으로 추가할 수 없습니다");
            return;
        }

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

        achievementdata.achievements.Add(newAhievement);
    }

    void SaveJSON()
    {
        int index = achievementDropdown.value;
        string id = idInput.text.Trim();

        if(string.IsNullOrEmpty(id))
        {
            Debug.LogWarning("업적 ID는 필수입니다.");
            return;
        }

        if (index == 0)
        { 
            AddNewAchievementFromFields();
            Debug.Log("새 업적이 추가 되었습니다.");
        }
        else
        {
            if (IsDuplicateID(id, index - 1))
            {
                Debug.LogWarning("업적 ID 중복으로 수정할 수 없습니다.");
                return;
            }

            ApplyFieldsToData(index - 1); // 업적 리스트는 0번이 없기 때문에 -1
            Debug.Log("업적이 수정되었습니다.");
        }

        AchievementSaveService.Save(achievementdata);
        RefreshUI();
        Debug.Log("업적 저장 완료");
    }

    void LoadJSON()
    {
        achievementdata = AchievementSaveService.Load();
        RefreshUI();
        Debug.Log("업적 불러오기 완료");
    }

    bool IsDuplicateID(string id, int ignoreIndex = -1)
    {
        for(int i = 0; i < achievementdata.achievements.Count; i++)
        {
            if (i == ignoreIndex) continue;
            if(achievementdata.achievements[i].id == id) return true;   
        }

        return false;
    }
}
