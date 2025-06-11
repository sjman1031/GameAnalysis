using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomJoinManager : MonoBehaviourPunCallbacks
{
    public InputField roomNumField;
    public GameObject errorPopUp;

    public void OnRoomJoinButton()
    {
        string roomNum = roomNumField.text;
        PhotonNetwork.JoinRoom(roomNum);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("05_Lobby");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        errorPopUp.SetActive(true);
    }

    public void OnBackButton() { SceneManager.LoadScene("01_MainMenu"); }
}