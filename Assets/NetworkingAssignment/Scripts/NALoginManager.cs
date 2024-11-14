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

    private bool isCreate = false;

    private void Start()
    {
        // NickName UI Ȱ��ȭ
        nickNameUIGo.SetActive(true);
        // RoomID UI ��Ȱ��ȭ
        roomIDUIGo.SetActive(false);
        
        isCreate = false;
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
            // ���� Ŭ���忡 ������ �����ϴ� ����
            // ���ӿ� �����ϸ� OnConnectedToMaster �޼��� ȣ��
            PhotonNetwork.ConnectUsingSettings();
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
            else
            {
                // ���� Ŭ���忡 ����Ǿ� ���� ���� ��� ���� Ŭ���忡 ������ ����
                Debug.LogFormat("Connect : {0}", gameVersion);

                PhotonNetwork.GameVersion = gameVersion;
                // ���ӿ� �����ϸ� OnConnectedToMaster �޼��� ȣ��
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

    // InputField_NickName�� ������ �г����� ������
    public void Connect()
    {
        Debug.Log("Connect Call!");
        // NickName UI Ȱ��ȭ
        nickNameUIGo.SetActive(false);
        // RoomID UI ��Ȱ��ȭ
        roomIDUIGo.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        // ���� Ŭ���忡 �ش� nickName���� �����ߴٴ� �α� ���
        Debug.LogFormat("Connected to Master: {0}", nickName);

        // ������/����� ��ư���� ��ȣ�ۿ� ��Ȱ��ȭ
        SwitchButtonActive();

        // ���� Ŭ���� �� ������ �뿡 ���� �õ�
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
        // �뿡�� ������ ������ ���
        Debug.LogWarningFormat("Disconnected: {0}", cause);

        createRoomButton.interactable = true;
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

        createRoomButton.interactable = true;
        joinRoomButton.interactable = true;
    }
}
