using UnityEngine;

using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

public class NAGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab = null;

    // 각 클라이언트 마다 생성된 플레이어 게임 오브젝트를 배열로 관리
    private GameObject[] playerGoList = new GameObject[4];

    private NAPlayerCtrl playerCtrl = null;
    private NAPlayerCtrl otherCtrl = null;
    private GameObject uiMngGo = null;
    private TextMeshProUGUI uiNRoom = null;
    private TextMeshProUGUI uiNPlayer = null;

    private void Start()
    {
        uiMngGo = FindAnyObjectByType<Canvas>().gameObject;
        uiNRoom = uiMngGo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        uiNPlayer = uiMngGo.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        uiNRoom.text = "Room ID : \nNULL";
        uiNPlayer.text = "Number of Player : \n0/4";

        if (playerPrefab != null)
        {
            //
            DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
            if (!pool.ResourceCache.ContainsKey(playerPrefab.name))
                pool.ResourceCache.Add(playerPrefab.name, playerPrefab);
            //

            Invoke("SpawnPlayer", 0.5f);
        }
    }

    private void SpawnPlayer()
    {
        GameObject go = PhotonNetwork.Instantiate(
                playerPrefab.name,
                new Vector3(
                    Random.Range(-10.0f, 10.0f),
                    0.0f,
                    Random.Range(-10.0f, 10.0f)),
                Quaternion.identity,
                0);
        playerCtrl = go.GetComponent<NAPlayerCtrl>();
        playerCtrl.SetMaterial(PhotonNetwork.CurrentRoom.PlayerCount);

        // Remote Procedure Call
        photonView.RPC("ApplyPlayerList", RpcTarget.All);
    }

    // PhotonNetwork.LeaveRooom 함수가 호출되면 호출
    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");

        SceneManager.LoadScene("LoginScene");
    }

    // 플레이어가 입장할 때 호출되는 함수
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.LogFormat("Player Entered Room: {0}",
                        otherPlayer.NickName);

        // 누군가 접속하면 전체 클라이언트에서 함수 호출
        //photonView.RPC("ApplyPlayerList", RpcTarget.All);
    }

    [PunRPC]
    public void ApplyPlayerList()
    {
        // 현재 방에 접속해 있는 플레이어의 수
        Debug.LogError("CurrentRoom PlayerCount : " + PhotonNetwork.CurrentRoom.PlayerCount);
        uiNPlayer.text = ("Number of Player : \n" + PhotonNetwork.CurrentRoom.PlayerCount + "/4").ToString();
        uiNRoom.text = ("RoomID :\n" + System.String.Format("{0:D5}", PhotonNetwork.CountOfRooms)).ToString();

        // 현재 생성되어 있는 모든 포톤뷰 가져오기
        //PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        PhotonView[] photonViews =
            FindObjectsByType<PhotonView>(FindObjectsSortMode.None);

        // 매번 재정렬을 하는게 좋으므로 플레이어 게임오브젝트 리스트를 초기화
        System.Array.Clear(playerGoList, 0, playerGoList.Length);

        // 현재 생성되어 있는 포톤뷰 전체와
        // 접속중인 플레이어들의 액터넘버를 비교해,
        // 액터넘버를 기준으로 플레이어 게임오브젝트 배열을 채움
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            // 키는 0이 아닌 1부터 시작
            int key = i + 1;
            for (int j = 0; j < photonViews.Length; ++j)
            {
                // 만약 PhotonNetwork.Instantiate를 통해서 생성된 포톤뷰가 아니라면 넘김
                if (photonViews[j].isRuntimeInstantiated == false) continue;
                // 만약 현재 키 값이 딕셔너리 내에 존재하지 않는다면 넘김
                if (PhotonNetwork.CurrentRoom.Players.ContainsKey(key) == false) continue;

                // 포톤뷰의 액터넘버
                int viewNum = photonViews[j].Owner.ActorNumber;

                // 다른 플레이어의 색상 부여 - 과제 1 (완료)
                otherCtrl = photonViews[j].gameObject.GetComponent<NAPlayerCtrl>();
                otherCtrl.SetMaterial(viewNum);

                // 접속중인 플레이어의 액터넘버
                int playerNum = PhotonNetwork.CurrentRoom.Players[key].ActorNumber;

                // 액터넘버가 같은 오브젝트가 있다면,
                if (viewNum == playerNum)
                {
                    // 실제 게임오브젝트를 배열에 추가
                    playerGoList[playerNum - 1] = photonViews[j].gameObject;
                    // 게임오브젝트 이름도 알아보기 쉽게 변경
                    playerGoList[playerNum - 1].name = "Player_" + photonViews[j].Owner.NickName;
                }
            }
        }

        // 디버그용
        PrintPlayerList();
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

    // 플레이어가 나갈 때 호출되는 함수
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.LogFormat("Player Left Room: {0}",
                        otherPlayer.NickName);
    }

    public void LeaveRoom()
    {
        Debug.Log("Leave Room");

        PhotonNetwork.LeaveRoom();
    }
}
