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

        DontDestroyOnLoad(gameObject);
    }
    
    public void OnNewButton() { Debug.Log("����ư"); }
    public void OnContinueButton() { }
    public void OnTogetherButton() { SceneManager.LoadScene("02_MultiPlaySelect"); }
    public void OnQuitButton() { Application.Quit(); }
    public void OnOptionQuitButton() { options.SetActive(false); }
    public void OnOptionButton() { options.SetActive(true); }
}
