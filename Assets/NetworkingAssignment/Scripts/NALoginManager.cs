using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LoginManager : MonoBehaviourPunCallbacks
{
    // ���� ���� �� �ִ� �÷��� �ο� �� Field
    [SerializeField] private string gameVersion = "0.0.1";
    [SerializeField] private byte maxPlyaerPerRoom = 4;

    // �г��� ����� Field
    [SerializeField] private string nickName = string.Empty;

    // ���ӿ� ��ư component
    [SerializeField] private Button connectButton = null;

    private void Start()
    {
        // ��ư Ȱ��ȭ
        connectButton.interactable = true;
    }

    // Connect Button�� �������� ȣ��
    public void Connect()
    {
        // �г��� ĭ�� ��������� �α� ��ȯ
        if (string.IsNullOrEmpty(nickName))
        {
            Debug.Log("NickName is empty");
            return;
        }

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

    // InputField_NickName�� ������ �г����� ������
    public void OnValueChangedNickName(string _nickName)
    {
        // �Է¹��� �г����� Field�� ����
        nickName = _nickName;

        // ���� �̸� ����
        PhotonNetwork.NickName = nickName;
    }

    public override void OnConnectedToMaster()
    {
        // ���� Ŭ���忡 �ش� nickName���� �����ߴٴ� �α� ���
        Debug.LogFormat("Connected to Master: {0}", nickName);

        // �α��� ��ư���� ��ȣ�ۿ� ��Ȱ��ȭ
        connectButton.interactable = false;

        // ���� Ŭ���� �� ������ �뿡 ���� �õ�
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // �뿡�� ������ ������ ���
        Debug.LogWarningFormat("Disconnected: {0}", cause);

        connectButton.interactable = true;
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

        connectButton.interactable = true;
        Debug.Log("Create Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlyaerPerRoom });
    }
}
