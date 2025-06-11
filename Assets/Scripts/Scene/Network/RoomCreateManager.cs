using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomCreateManager : MonoBehaviourPunCallbacks
{
    public InputField roomNumberField;
    public GameObject errorPopUp;

    public void OnCreateRoomButton()
    {
        string roomName = roomNumberField.text;
        RoomOptions options = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(roomName, options);
    }

    public override void OnCreatedRoom()
    {
        SceneManager.LoadScene("05_Lobby");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorPopUp.SetActive(true);
    }

    public void OnBackButton() { SceneManager.LoadScene("01_MainMenu"); }
}
