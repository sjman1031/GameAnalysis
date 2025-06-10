using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject options;

    public void Awake()
    {
        if (options == null)
            options = GameObject.Find("OptionUI");
    }

    public void OnNewButton() { Debug.Log("´º¹öÆ°"); }
    public void OnContinueButton() { }
    public void OnTogetherButton() { SceneManager.LoadScene("02_MultiPlaySelect"); }
    public void OnOptionButton() { options.SetActive(true); }
    public void OnQuitButton() { Application.Quit(); }
}
