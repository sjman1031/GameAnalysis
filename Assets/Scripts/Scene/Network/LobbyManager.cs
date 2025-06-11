using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject startButton;
    public GameObject waitingUI;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        UpdateStartButton();
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateStartButton();
    }

    private void UpdateStartButton()
    {
        bool showStart = PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2;

        startButton.SetActive(showStart);
        waitingUI.SetActive(!showStart && PhotonNetwork.CurrentRoom.PlayerCount == 2);
    }

    public void OnStartGameButton()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PhotonNetwork.LoadLevel("06_StageSelect");
    }

    public void OnLeaveButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("01_MainMenu");
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}