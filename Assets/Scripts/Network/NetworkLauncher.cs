using Photon.Pun;
using UnityEngine;

public class NetworkLauncher : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Master 서버에 접속
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();      // 로비 자동 입장
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 입장 완료");
    }
}
