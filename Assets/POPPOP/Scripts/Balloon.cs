using UnityEngine;
using Photon.Pun; // Photon ��Ʈ��ũ ���

public class Balloon : MonoBehaviourPun
{
    public int scoreValue = 10;       // ǳ���� ����
    public float lifespan = 3f;      // ǳ���� ���� (�� �� �� ����)
    private AudioSource popSound;    // ǳ�� ���� �� ���� �Ҹ�
    private bool isPopped = false;   // ǳ���� �̹� �������� ����

    private void Start()
    {
        popSound = GetComponent<AudioSource>(); // ����� �ҽ� ��������
        Destroy(gameObject, lifespan);          // ǳ���� ������ ���ϸ� �ڵ� ����
    }

    private void OnMouseDown()
    {
        if (isPopped) return; // �̹� ���� ǳ���� ó������ ����
        isPopped = true;      // ǳ���� �������� ǥ��

        GameManager gameManager = FindObjectOfType<GameManager>(); // GameManager ã��
        if (gameManager != null)
        {
            if (PhotonNetwork.IsConnected && photonView.IsMine) // ��Ʈ��ũ �÷��̾� ���� �߰�
            {
                gameManager.AddScore(PhotonNetwork.NickName, scoreValue);
            }
            else
            {
                gameManager.AddScore("LocalPlayer", scoreValue); // ���� �÷��̾� ���� �߰�
            }
        }

        // ǳ�� �ð��� ����
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false; // ǳ���� �ð������� ����
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false; // Ŭ�� �Ұ����ϰ� ����
        }

        // �Ҹ� ��� �� ����
        if (popSound != null)
        {
            popSound.Play(); // �Ҹ� ���
            Destroy(gameObject, popSound.clip.length); // �Ҹ� ���� �� ������Ʈ ����
        }
        else
        {
            Destroy(gameObject); // �Ҹ��� ������ ��� ����
        }
    }
}
