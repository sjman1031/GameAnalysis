using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    public SceneLoader loader;

    private void Start()
    {
        loader = GetComponent<SceneLoader>();

        int idx = StageLoader.SelectedStage;
        loader.LoadMap($"stage{idx}");
    }


}
