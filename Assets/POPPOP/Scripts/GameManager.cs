
using UnityEngine;
using Photon.Pun; // Photon ��Ʈ��ũ�� ����ϴ� ��� ���
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject[] basicBalloons; // �⺻ ǳ�� ������ �迭 (3��)
    public GameObject[] fastBalloons;  // ���� ǳ�� ������ �迭 (2��)
    public GameObject minusBalloon;    // ���̳ʽ� ǳ�� ������ (1��)

    public TextMeshProUGUI scoreText;     // ���� ������ ǥ���� �ؽ�Ʈ UI
    public GameObject resultPanel;        // ���� ���� �� ��Ÿ�� ��� �г�
    public TextMeshProUGUI resultText;    // ���� ������ ǥ���� �ؽ�Ʈ UI

    private int score = 0;               // ���� �÷��̾��� ����
    private float gameTime = 60f;        // ���� ���� �ð� (60�ʷ� ����)
    private bool isMultiplayer = false;  // ��Ƽ�÷��� ȯ������ ���� Ȯ�ο� ����

    public GameObject endGameParticles; // ��ƼŬ ������Ʈ �߰�

    void Start()
    {
        resultPanel.SetActive(false); // ������ �� ��� �г��� �����

        endGameParticles.SetActive(false); // ������ �� ��ƼŬ ��Ȱ��ȭ

        scoreText.gameObject.SetActive(false); // ���ھ� �ؽ�Ʈ ��Ȱ��ȭ

        // Photon�� ����� ��Ƽ�÷��̾� ȯ������ Ȯ��
        isMultiplayer = PhotonNetwork.IsConnected;
        // 1�� �Ŀ� ���ھ� �ؽ�Ʈ Ȱ��ȭ �� ǳ�� ���� ����
        Invoke("ShowScoreAndStartSpawning", 1f);
    }

    void ShowScoreAndStartSpawning()
    {
        // ���ھ� �ؽ�Ʈ�� ���̰� ����
        scoreText.gameObject.SetActive(true);

        // �ֱ������� ǳ�� ���� ����
        InvokeRepeating("SpawnBalloon", 0f, 2f); // �ٷ� ����, 2�� �������� �ݺ�
    }

    void Update()
    {
        gameTime -= Time.deltaTime; // �� �����Ӹ��� ���� �ð��� ���̱�

        if (gameTime <= 0)
        {
            EndGame(); // �ð��� �� �Ǹ� ���� ����
        }
    }

    // �������� ǳ���� �����ϴ� �Լ�
    void SpawnBalloon()
    {
        Debug.Log("Attempting to spawn balloon"); // ǳ�� ���� �õ� Ȯ�ο� �޽���

        GameObject balloon;
        float randomValue = Random.value;

        if (randomValue < 0.5f)
        {
            int index = Random.Range(0, basicBalloons.Length);
            balloon = isMultiplayer
                      ? PhotonNetwork.Instantiate(basicBalloons[index].name, RandomPosition(), Quaternion.identity)
                      : Instantiate(basicBalloons[index], RandomPosition(), Quaternion.identity);
            Debug.Log("Basic balloon spawned"); // �⺻ ǳ�� ���� Ȯ��
        }
        else if (randomValue < 0.8f)
        {
            int index = Random.Range(0, fastBalloons.Length);
            balloon = isMultiplayer
                      ? PhotonNetwork.Instantiate(fastBalloons[index].name, RandomPosition(), Quaternion.identity)
                      : Instantiate(fastBalloons[index], RandomPosition(), Quaternion.identity);
            Debug.Log("Fast balloon spawned"); // ���� ǳ�� ���� Ȯ��
        }
        else
        {
            balloon = isMultiplayer
                      ? PhotonNetwork.Instantiate(minusBalloon.name, RandomPosition(), Quaternion.identity)
                      : Instantiate(minusBalloon, RandomPosition(), Quaternion.identity);
            Debug.Log("Minus balloon spawned"); // ���̳ʽ� ǳ�� ���� Ȯ��
        }
    }

    // ǳ���� ������ ������ ��ġ�� ��ȯ�ϴ� �Լ�
    Vector3 RandomPosition()
    {
        float x = Random.Range(-8f, 8f);
        float y = Random.Range(-4f, 4f);
        return new Vector3(x, y, 0);
    }

    // ������ �߰��ϰ� UI�� �����ϴ� �Լ�
    public void AddScore(int amount)
    {
        score += amount; // ǳ�� ���� �߰�
        scoreText.text = "Score: " + score; // ���ŵ� ������ UI�� ǥ��
    }

    // ���� ���� �� ȣ��Ǵ� �Լ�
    void EndGame()
    {
        resultPanel.SetActive(true); // ��� �г� ǥ��
        resultText.text = "Final Score: " + score; // ���� ���� ǥ��
        CancelInvoke("SpawnBalloon"); // ǳ�� ���� ����

        endGameParticles.SetActive(true); // ������ ���� �� ��ƼŬ Ȱ��ȭ
    }
}