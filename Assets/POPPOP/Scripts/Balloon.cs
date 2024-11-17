using UnityEngine;
using Photon.Pun; // Photon 네트워크 사용

public class Balloon : MonoBehaviourPun
{
    public int scoreValue = 10;       // 풍선의 점수
    public float lifespan = 3f;      // 풍선의 수명 (몇 초 후 삭제)
    private AudioSource popSound;    // 풍선 터질 때 나는 소리
    private bool isPopped = false;   // 풍선이 이미 터졌는지 여부

    private void Start()
    {
        popSound = GetComponent<AudioSource>(); // 오디오 소스 가져오기
        Destroy(gameObject, lifespan);          // 풍선이 수명을 다하면 자동 삭제
    }

    private void OnMouseDown()
    {
        if (isPopped) return; // 이미 터진 풍선은 처리하지 않음
        isPopped = true;      // 풍선이 터졌음을 표시

        GameManager gameManager = FindObjectOfType<GameManager>(); // GameManager 찾기
        if (gameManager != null)
        {
            if (PhotonNetwork.IsConnected && photonView.IsMine) // 네트워크 플레이어 점수 추가
            {
                gameManager.AddScore(PhotonNetwork.NickName, scoreValue);
            }
            else
            {
                gameManager.AddScore("LocalPlayer", scoreValue); // 로컬 플레이어 점수 추가
            }
        }

        // 풍선 시각적 삭제
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false; // 풍선을 시각적으로 숨김
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false; // 클릭 불가능하게 설정
        }

        // 소리 재생 후 삭제
        if (popSound != null)
        {
            popSound.Play(); // 소리 재생
            Destroy(gameObject, popSound.clip.length); // 소리 끝난 후 오브젝트 삭제
        }
        else
        {
            Destroy(gameObject); // 소리가 없으면 즉시 삭제
        }
    }
}
