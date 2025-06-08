using Photon.Pun;
using UnityEngine;

public class NetworkLauncher : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Master ������ ����
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();      // �κ� �ڵ� ����
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� ���� �Ϸ�");
    }
}
