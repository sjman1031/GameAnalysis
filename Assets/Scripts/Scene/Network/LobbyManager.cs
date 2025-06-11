using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject startButton;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        UpdateStartButton();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateStartButton();
    }

    private void UpdateStartButton()
    {
        startButton.SetActive(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2);
    }

    public void OnStartGameButton()
    {
        if (PhotonNetwork.IsMasterClient)
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
}