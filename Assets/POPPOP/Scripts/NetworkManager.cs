
using Photon.Pun;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        Debug.Log("Connecting to Photon..."); // 연결 시도 메시지
        PhotonNetwork.ConnectUsingSettings(); // Photon 서버에 연결
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server"); // 서버 연결 성공 메시지
        PhotonNetwork.JoinLobby(); // 로비에 접속
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Photon Lobby"); // 로비 접속 성공 메시지
        PhotonNetwork.JoinOrCreateRoom("GameRoom", new Photon.Realtime.RoomOptions(), null); // 방 생성 또는 입장
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room"); // 방 입장 성공 메시지
    }
}