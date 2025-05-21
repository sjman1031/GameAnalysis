using System.Collections;
using UnityEngine;

public class CouroutineRunner : MonoBehaviour
{
    private static CouroutineRunner _instance;

    public static CouroutineRunner Instance
    {
        get
        {
            if(_instance == null)
            {
                var go = new GameObject("CoroutineRunner");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<CouroutineRunner>();
            }

            return _instance;   
        }
    }

    public void Run(IEnumerator routine) => StartCoroutine(routine);
}