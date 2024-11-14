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

    // RoomID 저장용 Field
    [SerializeField] private string RoomID = string.Empty;

    // 닉네임 저장용 Field
    [SerializeField] private string nickName = string.Empty;

    // 방 생성 및 방 참가 버튼 컴포넌트
    [SerializeField] private Button joinRoomButton = null;
    [SerializeField] private Button createRoomButton = null;

    [SerializeField] private GameObject nickNameUIGo = null;
    [SerializeField] private GameObject roomIDUIGo = null;

    private bool isCreate = false;

    private void Start()
    {
        // NickName UI 활성화
        nickNameUIGo.SetActive(true);
        // RoomID UI 비활성화
        roomIDUIGo.SetActive(false);
        
        isCreate = false;
    }

    // InputField_NickName과 연결해 닉네임을 가져옴
    public void OnValueChangedNickName(string _nickName)
    {
        nickName = _nickName;

        // 유저 이름 지정
        PhotonNetwork.NickName = nickName;
    }

    public void OnValueChangedRoomID(string _roomID)
    {
        RoomID = _roomID;
    }

    // 방생성 and 방참가 버튼 활성화/비활성화 메소드
    private void SwitchButtonActive()
    {
        createRoomButton.interactable = !createRoomButton.interactable;
        joinRoomButton.interactable = !joinRoomButton.interactable;
    }
    public void CreateRoom()
    {
        isCreate = true;
        if (string.IsNullOrEmpty(nickName))
        {
            isCreate = false;

            Debug.Log("NickName is empty");
            return;
        }

        if (PhotonNetwork.IsConnected)
        {
            SwitchButtonActive();
            Debug.Log("Create Room");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlyaerPerRoom });
        }
        else
        {
            Debug.LogFormat("Connect : {0}", gameVersion);

            PhotonNetwork.GameVersion = gameVersion;
            // 포톤 클라우드에 접속을 시작하는 지점
            // 접속에 성공하면 OnConnectedToMaster 메서드 호출
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    // Connect Button이 눌러지면 호출
    public void JoinRoom()
    {
        isCreate = false;
        if (string.IsNullOrEmpty(nickName))
        {
            Debug.Log("NickName is empty");
            return;
        }

        if (string.IsNullOrEmpty(RoomID))
        {
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
        else if(RoomID.Length < 5)
        {
            Debug.Log("RoomID must Numbers of 5!");
            return;
        }
        else
        {
            if (PhotonNetwork.IsConnected)
                PhotonNetwork.JoinRoom(RoomID);
            else
            {
                Debug.LogWarning("Join but No Connection master!");
                return;
            }
        }
    }

    // InputField_NickName과 연결해 닉네임을 가져옴
    public void Connect()
    {
        Debug.Log("Connect Call!");
        // NickName UI 활성화
        nickNameUIGo.SetActive(false);
        // RoomID UI 비활성화
        roomIDUIGo.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        // 포톤 클라우드에 해당 nickName으로 접속했다는 로그 출력
        Debug.LogFormat("Connected to Master: {0}", nickName);

        // 방참가/방생성 버튼과의 상호작용 비활성화
        SwitchButtonActive();

        // 포톤 클라우드 내 랜덤한 룸에 접속 시도
        if (isCreate)
        {
            Debug.Log("Create Room");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlyaerPerRoom });
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // 룸에서 연결이 끊어진 경우
        Debug.LogWarningFormat("Disconnected: {0}", cause);

        createRoomButton.interactable = true;
    }
    public override void OnJoinedRoom()
    {
        // 룸에 입장 성공했다는 로그를 출력.
        Debug.Log("Joined Room");

        // 로비에서 모여서 게임을 시작하게 될 것이므로 조인하는 대로 메인로비 씬으로 이동.
        SceneManager.LoadScene("MainLobby");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("JoinRandomFailed({0}): {1}", returnCode, message);

        createRoomButton.interactable = true;
        joinRoomButton.interactable = true;
    }
}
