using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using Photon.Realtime;

public class NALobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerUIPrefab; // UI 항목용 프리팹 (TextMeshPro 포함)
    [SerializeField] private Transform userListParent; // UI 목록의 부모 Transform
    [SerializeField] private GameObject startImage; // "Now Start" UI 이미지
    [SerializeField] private float displayDuration = 3f; // 이미지 표시 시간

    private void Start()
    {
        // 시작 이미지를 숨김
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
        // 기존 UI 닉네임 항목 모두 삭제
        foreach (Transform child in userListParent)
        {
            Destroy(child.gameObject);
        }

        // Room 내 모든 플레이어의 UI를 생성
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerUI = Instantiate(playerUIPrefab, userListParent); // 프리팹 생성
            var textComponent = playerUI.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = player.NickName; // 닉네임 설정
        }
    }

    private void UpdateRoomInfo()
    {
        // Room 이름과 플레이어 수 갱신
        var roomNameText = GameObject.Find("UIRoomNum").GetComponent<TextMeshProUGUI>();
        var playerCountText = GameObject.Find("UIPlayerNum").GetComponent<TextMeshProUGUI>();

        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        playerCountText.text = PhotonNetwork.CurrentRoom.PlayerCount + "/3";
    }

    public void TempNext()
    {
        // 방장이 게임 시작을 호출
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
