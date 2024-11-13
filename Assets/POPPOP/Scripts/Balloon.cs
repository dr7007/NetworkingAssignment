
using UnityEngine;
using Photon.Pun;

public class Balloon : MonoBehaviourPun
{
    public int scoreValue = 10; // 풍선 종류에 따라 점수 설정
    public float lifespan = 3f; // 풍선의 기본 수명, 빠른 풍선은 더 짧게 설정

    private void Start()
    {
        // 일정 시간이 지나면 풍선 자동 삭제
        Invoke("Disappear", lifespan);
    }

    private void OnMouseDown()
    {
        // 풍선 클릭 시 점수 추가 및 파괴
        GameObject.FindObjectOfType<GameManager>().AddScore(scoreValue);
        PhotonNetwork.Destroy(gameObject);
    }

    private void Disappear()
    {
        // 지정된 수명 이후 자동 삭제
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
