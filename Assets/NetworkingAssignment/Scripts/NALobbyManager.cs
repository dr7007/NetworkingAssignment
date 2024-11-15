using UnityEngine;

using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

public class NALobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab = null;

    // �� Ŭ���̾�Ʈ ���� ������ �÷��̾� ���� ������Ʈ�� �迭�� ����
    private GameObject[] playerGoList = new GameObject[3];

    private NAPlayerCtrl playerCtrl = null;

    private GameObject uiMngGo = null;
    private TextMeshProUGUI uiNRoom = null;
    private TextMeshProUGUI uiNPlayer = null;

    private void Start()
    {
        if (playerPrefab != null)
        {
            //
            DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
            if (!pool.ResourceCache.ContainsKey(playerPrefab.name))
                pool.ResourceCache.Add(playerPrefab.name, playerPrefab);
            //

            Invoke("SpawnPlayer", 0.5f);
        }

        uiMngGo = FindAnyObjectByType<Canvas>().gameObject;
        uiNRoom = uiMngGo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        uiNPlayer = uiMngGo.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        uiNRoom.text = ("RoomID :\n" + PhotonNetwork.CurrentRoom.Name).ToString();
        uiNPlayer.text = ("Number of Player : \n" + PhotonNetwork.CurrentRoom.PlayerCount + "/3").ToString();

    }

    private void SpawnPlayer()
    {
        GameObject go = PhotonNetwork.Instantiate(
                playerPrefab.name,
                new Vector3(
                    0.0f,
                    0.0f,
                    0.0f),
                Quaternion.identity,
                0);
        playerCtrl = go.GetComponent<NAPlayerCtrl>();




        // Remote Procedure Call
        photonView.RPC("ApplyPlayerList", RpcTarget.All);
    }

    // PhotonNetwork.LeaveRooom �Լ��� ȣ��Ǹ� ȣ��
    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");

        SceneManager.LoadScene("LoginScene");
    }

    // �÷��̾ ������ �� ȣ��Ǵ� �Լ�
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player otherPlayer)
    {
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        if (!pool.ResourceCache.ContainsKey(playerPrefab.name))
            pool.ResourceCache.Add(playerPrefab.name, playerPrefab);

        Debug.LogFormat("Player Entered Room: {0}",
                        otherPlayer.NickName);

        uiNPlayer.text = ("Number of Player : \n" + PhotonNetwork.CurrentRoom.PlayerCount + "/3").ToString();

        // ������ �����ϸ� ��ü Ŭ���̾�Ʈ���� �Լ� ȣ��
        //photonView.RPC("ApplyPlayerList", RpcTarget.All);
    }

    [PunRPC]
    public void ApplyPlayerList()
    {
        // ���� �濡 ������ �ִ� �÷��̾��� �� �α׿� ǥ��
        Debug.LogError("CurrentRoom PlayerCount : " + PhotonNetwork.CurrentRoom.PlayerCount);

        // ���� �����Ǿ� �ִ� ��� ����� ��������
        PhotonView[] photonViews =
            FindObjectsByType<PhotonView>(FindObjectsSortMode.None);

        // �Ź� �������� �ϴ°� �����Ƿ� �÷��̾� ���ӿ�����Ʈ ����Ʈ�� �ʱ�ȭ
        System.Array.Clear(playerGoList, 0, playerGoList.Length);

        // ���� �����Ǿ� �ִ� ����� ��ü��
        // �������� �÷��̾���� ���ͳѹ��� ����,
        // ���ͳѹ��� �������� �÷��̾� ���ӿ�����Ʈ �迭�� ä��
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            // Ű�� 0�� �ƴ� 1���� ����
            int key = i + 1;
            for (int j = 0; j < photonViews.Length; ++j)
            {
                // ���� PhotonNetwork.Instantiate�� ���ؼ� ������ ����䰡 �ƴ϶�� �ѱ�
                if (photonViews[j].isRuntimeInstantiated == false) continue;
                // ���� ���� Ű ���� ��ųʸ� ���� �������� �ʴ´ٸ� �ѱ�
                if (PhotonNetwork.CurrentRoom.Players.ContainsKey(key) == false) continue;

                // ������� ���ͳѹ�
                int viewNum = photonViews[j].Owner.ActorNumber;

                // �������� �÷��̾��� ���ͳѹ�
                int playerNum = PhotonNetwork.CurrentRoom.Players[key].ActorNumber;

                // ���ͳѹ��� ���� ������Ʈ�� �ִٸ�,
                if (viewNum == playerNum)
                {
                    // ���� ���ӿ�����Ʈ�� �迭�� �߰�
                    playerGoList[playerNum - 1] = photonViews[j].gameObject;
                    // ���ӿ�����Ʈ �̸��� �˾ƺ��� ���� ����
                    playerGoList[playerNum - 1].name = "Player_" + photonViews[j].Owner.NickName;

                }
            }
        }

        // ����׿�
        PrintPlayerList();

        // ���� ������Ʈ ��ġ ����
        SetPositionPlayer();
    }

    private void SetPositionPlayer()
    {
        foreach(GameObject player in playerGoList)
        {

        }
    }
    private void PrintPlayerList()
    {
        foreach (GameObject go in playerGoList)
        {
            if (go != null)
            {
                Debug.LogError(go.name);
            }
        }
    }

    // �÷��̾ ���� �� ȣ��Ǵ� �Լ�
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.LogFormat("Player Left Room: {0}",
                        otherPlayer.NickName);
        uiNPlayer.text = ("Number of Player : \n" + PhotonNetwork.CurrentRoom.PlayerCount + "/3").ToString();
    }

    public void LeaveRoom()
    {
        Debug.Log("Leave Room");

        PhotonNetwork.LeaveRoom();
    }
}
