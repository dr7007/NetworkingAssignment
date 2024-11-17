using UnityEngine;
using TMPro; // TextMeshPro 사용
using System.Collections.Generic; // 점수 저장 및 정렬용
using UnityEngine.SceneManagement; // 씬 전환을 위해 추가
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    // TextMeshPro UI 관련 변수들
    public TextMeshProUGUI scoreText;       // 현재 점수를 표시하는 UI
    public GameObject resultPanel;         // 본인 점수를 표시하는 결과 패널
    public TextMeshProUGUI resultText;     // 본인 최종 점수를 표시하는 UI
    public GameObject rankingPanel;        // 다른 플레이어 점수를 표시하는 랭킹 패널
    public TextMeshProUGUI rankingText;    // 랭킹 정보를 표시하는 TextMeshPro UI
    public GameObject endGameParticles;    // 게임 종료 시 나타날 파티클 효과
    public GameObject StartCanvas;

    // 풍선 관련 변수들 (Inspector에서 프리팹 연결)
    public GameObject[] basicBalloons;     // 기본 풍선 프리팹 배열 (3개)
    public GameObject[] fastBalloons;      // 빠른 풍선 프리팹 배열 (2개)
    public GameObject minusBalloon;        // 마이너스 풍선 프리팹 (1개)

    // 게임 및 점수 관리
    private Dictionary<string, int> playerScores = new Dictionary<string, int>(); // 플레이어 점수 저장
    private float gameTime = 60f;          // 제한 시간 60초
    private bool gameEnded = false;        // 게임 종료 여부

    private void Start()
    {
        // 게임 시작 시 초기화 작업\
        StartCanvas.SetActive(true);
        resultPanel.SetActive(false);      // 결과 패널 숨기기
        rankingPanel.SetActive(false);    // 랭킹 패널 숨기기
        endGameParticles.SetActive(false); // 파티클 효과 숨기기

        // 일정 간격으로 풍선 생성
        InvokeRepeating("SpawnBalloon", 1f, 0.5f); // 1초 후부터 n초 간격으로 호출
    }

    private void Update()
    {
        // 게임이 이미 끝난 경우 업데이트 중단
        if (gameEnded) return;

        // 게임 제한 시간 감소
        gameTime -= Time.deltaTime;

        // 시간이 다 되면 게임 종료
        if (gameTime <= 0)
        {
            EndGame();
        }
    }

    // 풍선을 무작위로 생성하는 함수
    public void SpawnBalloon()
    {
        if(StartCanvas.activeInHierarchy)
            StartCanvas.SetActive(false);
        // 프리팹 배열이 제대로 설정되지 않은 경우 에러 출력
        if (basicBalloons.Length == 0 || fastBalloons.Length == 0 || minusBalloon == null)
        {
            Debug.LogError("풍선 프리팹이 제대로 설정되지 않았습니다!");
            return;
        }

        GameObject balloon;                // 생성할 풍선 오브젝트
        float randomValue = Random.value;  // 0과 1 사이의 랜덤 값 생성

        // 풍선 종류를 랜덤 값에 따라 선택
        if (randomValue < 0.5f)
        {
            // 기본 풍선 생성
            int index = Random.Range(0, basicBalloons.Length);
            balloon = Instantiate(basicBalloons[index], RandomPosition(), Quaternion.identity);
        }
        else if (randomValue < 0.8f)
        {
            // 빠른 풍선 생성
            int index = Random.Range(0, fastBalloons.Length);
            balloon = Instantiate(fastBalloons[index], RandomPosition(), Quaternion.identity);
        }
        else
        {
            // 마이너스 풍선 생성
            balloon = Instantiate(minusBalloon, RandomPosition(), Quaternion.identity);
        }
    }

    // 풍선이 생성될 무작위 위치를 반환하는 함수
    private Vector3 RandomPosition()
    {
        float x = Random.Range(-8f, 8f);   // x 좌표는 -8에서 8 사이 랜덤
        float y = Random.Range(-4f, 4f);   // y 좌표는 -4에서 4 사이 랜덤
        return new Vector3(x, y, 0);       // 생성 위치 반환
    }

    // 점수를 추가하는 함수
    public void AddScore(string playerName, int score)
    {
        // 플레이어가 딕셔너리에 없으면 추가
        if (!playerScores.ContainsKey(playerName))
        {
            playerScores[playerName] = 0;
        }

        // 점수 업데이트
        playerScores[playerName] += score;

        // 로컬 플레이어 점수 UI 업데이트
        if (playerName == "LocalPlayer")
        {
            scoreText.text = "Score: " + playerScores[playerName];
        }
    }

    // 게임 종료 처리
    private void EndGame()
    {
        gameEnded = true;                  // 게임 종료 상태로 설정
        CancelInvoke("SpawnBalloon");      // 풍선 생성 중단

        // 결과 패널 활성화 및 점수 표시
        resultPanel.SetActive(true);
        resultText.text = "Final Score: " + (playerScores.ContainsKey("LocalPlayer") ? playerScores["LocalPlayer"] : 0);

        endGameParticles.SetActive(true);  // 게임 종료 후 파티클 효과 활성화

        // 3초 뒤 랭킹 보드 표시
        Invoke("ShowRankingBoard", 3f);
    }

    // 랭킹 보드를 표시하는 함수
    private void ShowRankingBoard()
    {
        resultPanel.SetActive(false);      // 결과 패널 숨기기
        rankingPanel.SetActive(true);      // 랭킹 패널 활성화
        rankingText.text = "Rankings:\n";  // 랭킹 초기화

        // 점수를 내림차순으로 정렬
        var sortedScores = new List<KeyValuePair<string, int>>(playerScores);
        sortedScores.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

        // 정렬된 점수를 UI에 출력
        int rank = 1;
        foreach (var entry in sortedScores)
        {
            rankingText.text += $"{rank}. {entry.Key}: {entry.Value}\n";
            rank++;
        }
    }


    // 메인 타이틀로 돌아가는 함수
    public void ReturnToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("LoginScene"); // "LoginScene" 씬으로 이동
    }

}
