using UnityEngine;
using TMPro; // TextMeshPro ���
using System.Collections.Generic; // ���� ���� �� ���Ŀ�
using UnityEngine.SceneManagement; // �� ��ȯ�� ���� �߰�
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    // TextMeshPro UI ���� ������
    public TextMeshProUGUI scoreText;       // ���� ������ ǥ���ϴ� UI
    public GameObject resultPanel;         // ���� ������ ǥ���ϴ� ��� �г�
    public TextMeshProUGUI resultText;     // ���� ���� ������ ǥ���ϴ� UI
    public GameObject rankingPanel;        // �ٸ� �÷��̾� ������ ǥ���ϴ� ��ŷ �г�
    public TextMeshProUGUI rankingText;    // ��ŷ ������ ǥ���ϴ� TextMeshPro UI
    public GameObject endGameParticles;    // ���� ���� �� ��Ÿ�� ��ƼŬ ȿ��
    public GameObject StartCanvas;

    // ǳ�� ���� ������ (Inspector���� ������ ����)
    public GameObject[] basicBalloons;     // �⺻ ǳ�� ������ �迭 (3��)
    public GameObject[] fastBalloons;      // ���� ǳ�� ������ �迭 (2��)
    public GameObject minusBalloon;        // ���̳ʽ� ǳ�� ������ (1��)

    // ���� �� ���� ����
    private Dictionary<string, int> playerScores = new Dictionary<string, int>(); // �÷��̾� ���� ����
    private float gameTime = 60f;          // ���� �ð� 60��
    private bool gameEnded = false;        // ���� ���� ����

    private void Start()
    {
        // ���� ���� �� �ʱ�ȭ �۾�\
        StartCanvas.SetActive(true);
        resultPanel.SetActive(false);      // ��� �г� �����
        rankingPanel.SetActive(false);    // ��ŷ �г� �����
        endGameParticles.SetActive(false); // ��ƼŬ ȿ�� �����

        // ���� �������� ǳ�� ����
        InvokeRepeating("SpawnBalloon", 1f, 0.5f); // 1�� �ĺ��� n�� �������� ȣ��
    }

    private void Update()
    {
        // ������ �̹� ���� ��� ������Ʈ �ߴ�
        if (gameEnded) return;

        // ���� ���� �ð� ����
        gameTime -= Time.deltaTime;

        // �ð��� �� �Ǹ� ���� ����
        if (gameTime <= 0)
        {
            EndGame();
        }
    }

    // ǳ���� �������� �����ϴ� �Լ�
    public void SpawnBalloon()
    {
        if(StartCanvas.activeInHierarchy)
            StartCanvas.SetActive(false);
        // ������ �迭�� ����� �������� ���� ��� ���� ���
        if (basicBalloons.Length == 0 || fastBalloons.Length == 0 || minusBalloon == null)
        {
            Debug.LogError("ǳ�� �������� ����� �������� �ʾҽ��ϴ�!");
            return;
        }

        GameObject balloon;                // ������ ǳ�� ������Ʈ
        float randomValue = Random.value;  // 0�� 1 ������ ���� �� ����

        // ǳ�� ������ ���� ���� ���� ����
        if (randomValue < 0.5f)
        {
            // �⺻ ǳ�� ����
            int index = Random.Range(0, basicBalloons.Length);
            balloon = Instantiate(basicBalloons[index], RandomPosition(), Quaternion.identity);
        }
        else if (randomValue < 0.8f)
        {
            // ���� ǳ�� ����
            int index = Random.Range(0, fastBalloons.Length);
            balloon = Instantiate(fastBalloons[index], RandomPosition(), Quaternion.identity);
        }
        else
        {
            // ���̳ʽ� ǳ�� ����
            balloon = Instantiate(minusBalloon, RandomPosition(), Quaternion.identity);
        }
    }

    // ǳ���� ������ ������ ��ġ�� ��ȯ�ϴ� �Լ�
    private Vector3 RandomPosition()
    {
        float x = Random.Range(-8f, 8f);   // x ��ǥ�� -8���� 8 ���� ����
        float y = Random.Range(-4f, 4f);   // y ��ǥ�� -4���� 4 ���� ����
        return new Vector3(x, y, 0);       // ���� ��ġ ��ȯ
    }

    // ������ �߰��ϴ� �Լ�
    public void AddScore(string playerName, int score)
    {
        // �÷��̾ ��ųʸ��� ������ �߰�
        if (!playerScores.ContainsKey(playerName))
        {
            playerScores[playerName] = 0;
        }

        // ���� ������Ʈ
        playerScores[playerName] += score;

        // ���� �÷��̾� ���� UI ������Ʈ
        if (playerName == "LocalPlayer")
        {
            scoreText.text = "Score: " + playerScores[playerName];
        }
    }

    // ���� ���� ó��
    private void EndGame()
    {
        gameEnded = true;                  // ���� ���� ���·� ����
        CancelInvoke("SpawnBalloon");      // ǳ�� ���� �ߴ�

        // ��� �г� Ȱ��ȭ �� ���� ǥ��
        resultPanel.SetActive(true);
        resultText.text = "Final Score: " + (playerScores.ContainsKey("LocalPlayer") ? playerScores["LocalPlayer"] : 0);

        endGameParticles.SetActive(true);  // ���� ���� �� ��ƼŬ ȿ�� Ȱ��ȭ

        // 3�� �� ��ŷ ���� ǥ��
        Invoke("ShowRankingBoard", 3f);
    }

    // ��ŷ ���带 ǥ���ϴ� �Լ�
    private void ShowRankingBoard()
    {
        resultPanel.SetActive(false);      // ��� �г� �����
        rankingPanel.SetActive(true);      // ��ŷ �г� Ȱ��ȭ
        rankingText.text = "Rankings:\n";  // ��ŷ �ʱ�ȭ

        // ������ ������������ ����
        var sortedScores = new List<KeyValuePair<string, int>>(playerScores);
        sortedScores.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

        // ���ĵ� ������ UI�� ���
        int rank = 1;
        foreach (var entry in sortedScores)
        {
            rankingText.text += $"{rank}. {entry.Key}: {entry.Value}\n";
            rank++;
        }
    }


    // ���� Ÿ��Ʋ�� ���ư��� �Լ�
    public void ReturnToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("LoginScene"); // "LoginScene" ������ �̵�
    }

}
