using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LoginManager : MonoBehaviourPunCallbacks
{
    // 게임 버전 및 최대 플레이 인원 수 Field
    [SerializeField] private string gameVersion = "0.0.1";
    [SerializeField] private byte maxPlyaerPerRoom = 3;

    // 닉네임 저장용 Field
    [SerializeField] private string nickName = string.Empty;

    // 접속용 버튼 component
    [SerializeField] private Button connectButton = null;

    private void Start()
    {
        // 버튼 활성화
        connectButton.interactable = true;
    }

    // Connect Button이 눌러지면 호출
    public void Connect()
    {
        // 닉네임 칸이 비어있을시 로그 반환
        if (string.IsNullOrEmpty(nickName))
        {
            Debug.Log("NickName is empty");
            return;
        }

        // 포톤 클라우드에 연결되어 있을때 랜덤 룸에 입장시도
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.JoinRandomRoom();
        else
        {
            // 포톤 클라우드에 연결되어 있지 않은 경우 포톤 클라우드에 접속을 시작
            Debug.LogFormat("Connect : {0}", gameVersion);
            
            PhotonNetwork.GameVersion = gameVersion;
            // 접속에 성공하면 OnConnectedToMaster 메서드 호출
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // InputField_NickName과 연결해 닉네임을 가져옴
    public void OnValueChangedNickName(string _nickName)
    {
        nickName = _nickName;

        // 유저 이름 지정
        PhotonNetwork.NickName = nickName;
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogFormat("Connected to Master: {0}", nickName);

        connectButton.interactable = false;

        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Disconnected: {0}", cause);

        connectButton.interactable = true;
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");

        // 마스터가 동시에 게임을 시작하게하는 구조가 아니기 때문에 각자 씬을 부르면 됨
        
        SceneManager.LoadScene("MainLobby");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("JoinRandomFailed({0}): {1}", returnCode, message);

        connectButton.interactable = true;
        Debug.Log("Create Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlyaerPerRoom });
    }
}
