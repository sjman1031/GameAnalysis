using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviourPun, IOnEventCallback
{
    const byte STAGE_SELETED = 1;

    [SerializeField] public List<Button> stageButtons = new();

    private void Start()
    {
        PhotonNetwork.AddCallbackTarget(this);

        for(int i = 0; i < stageButtons.Count; i++)
        {
            int idx = i + 1;

            stageButtons[i].onClick.RemoveAllListeners();
            stageButtons[i].onClick.AddListener(() => OnStageChosen(idx));  
        }
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnStageChosen(int stageIndex)
    {
        StageLoader.SelectedStage = stageIndex;

        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            PhotonNetwork.RaiseEvent(
                STAGE_SELETED,
                stageIndex,
                new RaiseEventOptions { Receivers = ReceiverGroup.All },
                new SendOptions { Reliability = true }
            ); 
        }
        else
        {
            SceneManager.LoadScene("07_GamePlay");
        }
    }

    public void OnEvent(EventData e)
    {
        if (e.Code != STAGE_SELETED) return;
        int sel = (int)e.CustomData;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel("07_GamePlay");
    }
}
