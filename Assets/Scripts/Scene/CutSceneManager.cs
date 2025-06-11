using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> cutScenes;

    [SerializeField] private float commonDuration;

    private void Start()
    {
        foreach (var cs in cutScenes)
            cs.SetActive(true);

        StartCoroutine(PlayCutScenes());
    }

    private IEnumerator PlayCutScenes()
    {

        for(int i = 0; i < cutScenes.Count; i++)
        {
            if(cutScenes[i].activeSelf == false)
                cutScenes[i].SetActive(true);

            float elapsed = 0f;
            while (elapsed < commonDuration)
            {
                if (Input.anyKeyDown)
                    break;

                elapsed += Time.deltaTime;
                yield return null;
            }

            if (i < cutScenes.Count - 1)
            {
                cutScenes[i].SetActive(false);
                yield return null;
            }
        }

        SceneManager.LoadScene("06_StageSelect");
    }
}
