
using UnityEngine;
using Photon.Pun;

public class Balloon : MonoBehaviourPun
{
    public int scoreValue = 10; // ǳ�� ������ ���� ���� ����
    public float lifespan = 3f; // ǳ���� �⺻ ����, ���� ǳ���� �� ª�� ����

    private void Start()
    {
        // ���� �ð��� ������ ǳ�� �ڵ� ����
        Invoke("Disappear", lifespan);
    }

    private void OnMouseDown()
    {
        // ǳ�� Ŭ�� �� ���� �߰� �� �ı�
        GameObject.FindObjectOfType<GameManager>().AddScore(scoreValue);
        PhotonNetwork.Destroy(gameObject);
    }

    private void Disappear()
    {
        // ������ ���� ���� �ڵ� ����
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
