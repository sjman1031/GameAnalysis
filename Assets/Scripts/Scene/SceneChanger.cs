using Photon.Pun;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviourPunCallbacks
{
    public void OnCreateLobby() { SceneManager.LoadScene("03_RoomCreate"); }
    public void OnJoinLobby() { SceneManager.LoadScene("04_RoomJoin"); }
}
