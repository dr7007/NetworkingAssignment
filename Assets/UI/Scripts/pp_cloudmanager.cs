using UnityEngine;

public class CloudManager : MonoBehaviour
{
    [System.Serializable]
    public class Cloud
    {
        public GameObject cloudObject;  // 구름 오브젝트 (UI Image)
        public float speed = 1f;        // 구름 이동 속도
    }

    [SerializeField] private Cloud[] clouds;   // 여러 구름들을 관리할 배열
    [SerializeField] private float screenLeftEdge = -10f;   // 왼쪽 경계
    [SerializeField] private float screenRightEdge = 10f;   // 오른쪽 경계

    void Update()
    {
        MoveClouds();
    }

    private void MoveClouds()
    {
        foreach (var cloud in clouds)
        {
            if (cloud.cloudObject != null)
            {
                // 구름을 왼쪽으로 이동
                cloud.cloudObject.transform.Translate(Vector3.left * cloud.speed * Time.deltaTime);

                // 구름이 화면 왼쪽 경계를 넘어간 경우 오른쪽에서 다시 나타나도록 설정
                RectTransform rectTransform = cloud.cloudObject.GetComponent<RectTransform>();
                float cloudWidth = rectTransform.rect.width * rectTransform.lossyScale.x; // 구름의 실제 화면상 너비 계산

                if (cloud.cloudObject.transform.position.x < screenLeftEdge - cloudWidth)
                {
                    Vector3 newPosition = cloud.cloudObject.transform.position;
                    newPosition.x = screenRightEdge + cloudWidth;  // 오른쪽에서 다시 나타나게 위치 변경
                    cloud.cloudObject.transform.position = newPosition;
                }
            }
        }

    }



}