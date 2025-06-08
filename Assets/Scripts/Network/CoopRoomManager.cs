using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CoopRoomManager : MonoBehaviourPunCallbacks
{
    const byte MaxPlayers = 2;

    public void CreateOrJoinRoom()
    {
        string roomName = "CoopRoom";
        RoomOptions opts = new RoomOptions { MaxPlayers = MaxPlayers };
        PhotonNetwork.JoinOrCreateRoom(roomName, opts, TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"[{PhotonNetwork.CurrentRoom.PlayerCount}/{MaxPlayers}]∏Ì ¿‘¿Â");
        if(PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayers)
        {
            StartGame();
        }

    }

    void StartGame()
    {
        PhotonNetwork.LoadLevel("")
    }
}