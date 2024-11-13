
using Photon.Pun;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        Debug.Log("Connecting to Photon..."); // ���� �õ� �޽���
        PhotonNetwork.ConnectUsingSettings(); // Photon ������ ����
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server"); // ���� ���� ���� �޽���
        PhotonNetwork.JoinLobby(); // �κ� ����
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Photon Lobby"); // �κ� ���� ���� �޽���
        PhotonNetwork.JoinOrCreateRoom("GameRoom", new Photon.Realtime.RoomOptions(), null); // �� ���� �Ǵ� ����
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room"); // �� ���� ���� �޽���
    }
}