using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Assign in Inspector")]
    [SerializeField] private AudioSource bgmSource;  // Loop = true
    [SerializeField] private AudioSource sfxSource;  // PlayOneShot 용
    [SerializeField] private float sfxVolume = 1f;

    private const string MASTER_KEY = "MASTER_VOLUME";
    private const string BGM_KEY = "BGM_VOLUME";
    private const string SFX_KEY = "SFX_VOLUME";

    private void Awake()
    {
        // 싱글톤 & 파괴 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 저장된 설정 불러오기
        float master = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float bgm = PlayerPrefs.GetFloat(BGM_KEY, 0.75f);
        float sfx = PlayerPrefs.GetFloat(SFX_KEY, 0.75f);

        SetMaster(master);
        SetBGM(bgm);
        SetSFX(sfx);
    }

    public void SetMaster(float linear)
    {
        AudioListener.volume = Mathf.Clamp01(linear);
        PlayerPrefs.SetFloat(MASTER_KEY, linear);
    }

    public void SetBGM(float linear)
    {
        bgmSource.volume = Mathf.Clamp01(linear);
        PlayerPrefs.SetFloat(BGM_KEY, linear);
    }

    public void SetSFX(float linear)
    {
        sfxSource.volume = Mathf.Clamp01(linear);
        PlayerPrefs.SetFloat(SFX_KEY, linear);
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        sfxSource.PlayOneShot(clip, volumeScale);
    }

    public void PlaySFXAtPoint(AudioClip clip, Vector3 pos, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, pos, volume);
    }

}