using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class LoginManager : MonoBehaviourPunCallbacks
{
    // ���� ���� �� �ִ� �÷��� �ο� �� Field
    [SerializeField] private string gameVersion = "0.0.1";
    [SerializeField] private byte maxPlyaerPerRoom = 3;

    // RoomID ����� Field
    [SerializeField] private string RoomID = string.Empty;

    // �г��� ����� Field
    [SerializeField] private string nickName = string.Empty;

    // �� ���� �� �� ���� ��ư ������Ʈ
    [SerializeField] private Button joinRoomButton = null;
    [SerializeField] private Button createRoomButton = null;

    [SerializeField] private GameObject nickNameUIGo = null;
    [SerializeField] private GameObject roomIDUIGo = null;
    [SerializeField] private GameObject mainUIGo = null;

    private bool isCreate = false;
    private RoomOptions roomopt = new RoomOptions();
    private bool isOpening = true;
    
    

    private void Start()
    {
        mainUIGo.SetActive(true);
        // NickName UI Ȱ��ȭ
        nickNameUIGo.SetActive(false);
        // RoomID UI ��Ȱ��ȭ
        roomIDUIGo.SetActive(false);
        
        isCreate = false;
        roomopt.MaxPlayers = maxPlyaerPerRoom;
        isOpening = true;
    }
    private void Update()
    {
        if (isOpening)
        {
            if (Input.anyKeyDown)
            {
                isOpening = false;
                OpeningPhase();
            }
        }
    }

    private void OpeningPhase()
    {
        mainUIGo.SetActive(false);
        nickNameUIGo.SetActive(true);
    }

    // InputField_NickName�� ������ �г����� ������
    public void OnValueChangedNickName(string _nickName)
    {
        nickName = _nickName;

        // ���� �̸� ����
        PhotonNetwork.NickName = nickName;
    }

    public void OnValueChangedRoomID(string _roomID)
    {
        RoomID = _roomID;
    }

    // ����� and ������ ��ư Ȱ��ȭ/��Ȱ��ȭ �޼ҵ�
    private void SwitchButtonActive()
    {
        createRoomButton.interactable = !createRoomButton.interactable;
        joinRoomButton.interactable = !joinRoomButton.interactable;
    }

    private void ButtonActive()
    {
        createRoomButton.interactable = true;
        joinRoomButton.interactable = true;
    }

    private bool RoomIDCompare()
    {
        for(int i = 0; i < PhotonNetwork.CountOfRooms; ++i)
        {
            if (RoomID == System.String.Format("{0:D5}", i + 1))
                return true;
        }
        return false;
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
            Debug.Log("Create Room");
            PhotonNetwork.CreateRoom(System.String.Format("{0:D5}", PhotonNetwork.CountOfRooms + 1), roomopt);
            SwitchButtonActive();
        }
    }
    // Connect Button�� �������� ȣ��
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
            // ���� Ŭ���忡 ����Ǿ� ������ ���� �뿡 ����õ�
            if (PhotonNetwork.IsConnected)
                    PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            if (RoomIDCompare())
                PhotonNetwork.JoinRoom(RoomID);
            else
            {
                Debug.LogWarning("Couldn't Fine the RoomID!");
                return;
            }
        }
    }

    // InputField_NickName�� ������ �г����� ������
    public void Connect()
    {
        Debug.Log("Connect Call!");

        Debug.LogFormat("Connect : {0}", gameVersion);

        PhotonNetwork.GameVersion = gameVersion;
        // ���� Ŭ���忡 ������ �����ϴ� ����
        // ���ӿ� �����ϸ� OnConnectedToMaster �޼��� ȣ��
        PhotonNetwork.ConnectUsingSettings();

        // NickName UI Ȱ��ȭ
        nickNameUIGo.SetActive(false);
        // RoomID UI ��Ȱ��ȭ
        roomIDUIGo.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        // ���� Ŭ���忡 �ش� nickName���� �����ߴٴ� �α� ���
        Debug.LogFormat("Connected to Master: {0}", nickName);

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // �뿡�� ������ ������ ���
        Debug.LogWarningFormat("Disconnected: {0}", cause);

        ButtonActive();
    }
    public override void OnJoinedRoom()
    {
        // �뿡 ���� �����ߴٴ� �α׸� ���.
        Debug.Log("Joined Room");

        // �κ񿡼� �𿩼� ������ �����ϰ� �� ���̹Ƿ� �����ϴ� ��� ���ηκ� ������ �̵�.
        SceneManager.LoadScene("MainLobby");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogErrorFormat("JoinRandomFailed({0}): {1}", returnCode, message);

        ButtonActive();
    }
}
