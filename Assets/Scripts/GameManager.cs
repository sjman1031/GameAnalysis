using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MapClearData
{
    public int triedCount = 0;
    public float clearTime = 0f;
}


public class GameManager : MonoBehaviourPunCallbacks
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                Debug.LogError("GameManager 인스턴스가 존재하지 않습니다.");
            }

            return _instance;
        }
    }


    public Dictionary<string, List<MapClearData>> mapClearData = new Dictionary<string, List<MapClearData>>();

    public bool onStage = false;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(_instance != this)
            Destroy(gameObject);    
    }

    private void Start()
    {
        foreach (var pc in FindObjectsOfType<PlayerController>())
        {
            var pv = pc.GetComponent<PhotonView>();
            if (PhotonNetwork.IsMasterClient && pc.name == "Lucy")
            { 
                if (!pv.IsMine) pc.enabled = false;
            }
            else if (!PhotonNetwork.IsMasterClient && pc.name == "Paul")
            {
                pc.enabled = false;
            }
        }
    }

    public void SaveStageClear()
    {
    }
    
    public void OnExitButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("01_MainMenu");
    }

    private void Update()
    {
        if(onStage)

    }
}
