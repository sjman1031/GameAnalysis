using UnityEngine;

public class StageLoader : MonoBehaviour
{
    public static StageLoader Instance { get; private set; }
    public static int SelectedStage { get; set; }  // 숫자만 저장

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}