
using UnityEngine;
using Photon.Pun; // Photon 네트워크를 사용하는 경우 사용
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject[] basicBalloons; // 기본 풍선 프리팹 배열 (3개)
    public GameObject[] fastBalloons;  // 빠른 풍선 프리팹 배열 (2개)
    public GameObject minusBalloon;    // 마이너스 풍선 프리팹 (1개)

    public TextMeshProUGUI scoreText;     // 현재 점수를 표시할 텍스트 UI
    public GameObject resultPanel;        // 게임 종료 시 나타날 결과 패널
    public TextMeshProUGUI resultText;    // 최종 점수를 표시할 텍스트 UI

    private int score = 0;               // 현재 플레이어의 점수
    private float gameTime = 60f;        // 게임 제한 시간 (60초로 설정)
    private bool isMultiplayer = false;  // 멀티플레이 환경인지 여부 확인용 변수

    public GameObject endGameParticles; // 파티클 오브젝트 추가

    void Start()
    {
        resultPanel.SetActive(false); // 시작할 때 결과 패널을 숨기기

        endGameParticles.SetActive(false); // 시작할 때 파티클 비활성화

        scoreText.gameObject.SetActive(false); // 스코어 텍스트 비활성화

        // Photon이 연결된 멀티플레이어 환경인지 확인
        isMultiplayer = PhotonNetwork.IsConnected;
        // 1초 후에 스코어 텍스트 활성화 및 풍선 생성 시작
        Invoke("ShowScoreAndStartSpawning", 1f);
    }

    void ShowScoreAndStartSpawning()
    {
        // 스코어 텍스트를 보이게 설정
        scoreText.gameObject.SetActive(true);

        // 주기적으로 풍선 생성 시작
        InvokeRepeating("SpawnBalloon", 0f, 2f); // 바로 시작, 2초 간격으로 반복
    }

    void Update()
    {
        gameTime -= Time.deltaTime; // 매 프레임마다 게임 시간을 줄이기

        if (gameTime <= 0)
        {
            EndGame(); // 시간이 다 되면 게임 종료
        }
    }

    // 무작위로 풍선을 생성하는 함수
    void SpawnBalloon()
    {
        Debug.Log("Attempting to spawn balloon"); // 풍선 생성 시도 확인용 메시지

        GameObject balloon;
        float randomValue = Random.value;

        if (randomValue < 0.5f)
        {
            int index = Random.Range(0, basicBalloons.Length);
            balloon = isMultiplayer
                      ? PhotonNetwork.Instantiate(basicBalloons[index].name, RandomPosition(), Quaternion.identity)
                      : Instantiate(basicBalloons[index], RandomPosition(), Quaternion.identity);
            Debug.Log("Basic balloon spawned"); // 기본 풍선 생성 확인
        }
        else if (randomValue < 0.8f)
        {
            int index = Random.Range(0, fastBalloons.Length);
            balloon = isMultiplayer
                      ? PhotonNetwork.Instantiate(fastBalloons[index].name, RandomPosition(), Quaternion.identity)
                      : Instantiate(fastBalloons[index], RandomPosition(), Quaternion.identity);
            Debug.Log("Fast balloon spawned"); // 빠른 풍선 생성 확인
        }
        else
        {
            balloon = isMultiplayer
                      ? PhotonNetwork.Instantiate(minusBalloon.name, RandomPosition(), Quaternion.identity)
                      : Instantiate(minusBalloon, RandomPosition(), Quaternion.identity);
            Debug.Log("Minus balloon spawned"); // 마이너스 풍선 생성 확인
        }
    }

    // 풍선을 생성할 무작위 위치를 반환하는 함수
    Vector3 RandomPosition()
    {
        float x = Random.Range(-8f, 8f);
        float y = Random.Range(-4f, 4f);
        return new Vector3(x, y, 0);
    }

    // 점수를 추가하고 UI를 갱신하는 함수
    public void AddScore(int amount)
    {
        score += amount; // 풍선 점수 추가
        scoreText.text = "Score: " + score; // 갱신된 점수를 UI에 표시
    }

    // 게임 종료 시 호출되는 함수
    void EndGame()
    {
        resultPanel.SetActive(true); // 결과 패널 표시
        resultText.text = "Final Score: " + score; // 최종 점수 표시
        CancelInvoke("SpawnBalloon"); // 풍선 생성 중지

        endGameParticles.SetActive(true); // 게임이 끝날 때 파티클 활성화
    }
}