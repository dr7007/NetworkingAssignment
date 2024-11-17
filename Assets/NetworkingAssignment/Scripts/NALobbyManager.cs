using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using Photon.Realtime;

public class NALobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerUIPrefab; // UI �׸�� ������ (TextMeshPro ����)
    [SerializeField] private Transform userListParent; // UI ����� �θ� Transform
    [SerializeField] private GameObject startImage; // "Now Start" UI �̹���
    [SerializeField] private float displayDuration = 3f; // �̹��� ǥ�� �ð�

    private void Start()
    {
        // ���� �̹����� ����
        if (startImage != null)
            startImage.SetActive(false);

        UpdateRoomInfo();
        UpdatePlayerList();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdatePlayerList();
        UpdateRoomInfo();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        UpdatePlayerList();
        UpdateRoomInfo();
    }

    private void UpdatePlayerList()
    {
        // ���� UI �г��� �׸� ��� ����
        foreach (Transform child in userListParent)
        {
            Destroy(child.gameObject);
        }

        // Room �� ��� �÷��̾��� UI�� ����
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerUI = Instantiate(playerUIPrefab, userListParent); // ������ ����
            var textComponent = playerUI.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = player.NickName; // �г��� ����
        }
    }

    private void UpdateRoomInfo()
    {
        // Room �̸��� �÷��̾� �� ����
        var roomNameText = GameObject.Find("UIRoomNum").GetComponent<TextMeshProUGUI>();
        var playerCountText = GameObject.Find("UIPlayerNum").GetComponent<TextMeshProUGUI>();

        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        playerCountText.text = PhotonNetwork.CurrentRoom.PlayerCount + "/3";
    }

    public void TempNext()
    {
        // ������ ���� ������ ȣ��
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartGame", RpcTarget.All);
        }
        else
        {
            Debug.Log("Only the MasterClient can start the game.");
        }
    }

    [PunRPC]
    private void StartGame()
    {
        userListParent.gameObject.SetActive(false);
        FindFirstObjectByType<Canvas>().gameObject.SetActive(false);
        StartCoroutine(ShowStartImageAndLoadScene());
    }

    private IEnumerator ShowStartImageAndLoadScene()
    {
        if (startImage != null)
            startImage.SetActive(true);

        yield return new WaitForSeconds(displayDuration);

        if (startImage != null)
            startImage.SetActive(false);

        SceneManager.LoadScene("GameScene");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("LoginScene");
    }
}
