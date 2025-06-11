using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject options;

    public Slider soundSlider;
    public Slider bgmSilder;
    public Slider sfxSlider;

    public void Awake()
    {
        if (options == null)
            options = GameObject.Find("OptionUI");

        DontDestroyOnLoad(gameObject);
    }

    public void OnNewButton() { SceneManager.LoadScene("08_StartCutScene"); }
    public void OnContinueButton() { }
    public void OnTogetherButton() { SceneManager.LoadScene("02_MultiPlaySelect"); }
    public void OnQuitButton() { Application.Quit(); }
    public void OnOptionQuitButton() { options.SetActive(false); }
    public void OnOptionButton() { options.SetActive(true); }
    public void OnMasterSliderChanged(float v) { AudioManager.Instance.SetMaster(v); }
    public void OnBgmSliderChanged(float v) { AudioManager.Instance.SetBGM(v); }
    public void OnSfxSliderChanged(float v) { AudioManager.Instance.SetSFX(v); }
}
