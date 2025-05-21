using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("UI 연결")]
    public TMP_InputField inputField_MapName;
    public TMP_Dropdown dropdown_MapList;
    public Button button_Save;
    public Button button_Load;
    public Button button_Delete;

    [Header("SceneSaver 참조")]
    public SceneSaver mapSaver;

    [Header("SceneLoader 참조")]
    public SceneLoader mapLoader;

    private void Start()
    {
        // 버튼에 함수 연결
        button_Save.onClick.AddListener(OnClickSave);
        button_Load.onClick.AddListener(OnClickLoad);
        button_Delete.onClick.AddListener(OnClickDelete);

        RefreshDropDown();
    }

    public void OnClickSave()
    {
        string mapName = inputField_MapName.text.Trim();    
        
        if(string.IsNullOrEmpty(mapName))
        {
            Debug.LogWarning("맵 이름을 입력하세요.");
            return;
        }

        mapSaver.SaveMap(mapName);
        RefreshDropDown();
    }

    public void OnClickLoad()
    {
        if (dropdown_MapList.options.Count == 0) return;

        string selectedMap = dropdown_MapList.options[dropdown_MapList.value].text;
        mapLoader.LoadMap(selectedMap);    
    }

    public void OnClickDelete()
    {
        if (dropdown_MapList.options.Count == 0) return;

        string selected = dropdown_MapList.options[dropdown_MapList.value].text;
        mapSaver.DeleteMap(selected);
        RefreshDropDown();
    }

    public void RefreshDropDown()
    {
        dropdown_MapList.ClearOptions();
        List<string> mapNames = mapSaver.GetMapNames();
        dropdown_MapList.AddOptions(mapNames);

        if (mapNames.Count > 0)
            dropdown_MapList.value = 0;
    }
}
