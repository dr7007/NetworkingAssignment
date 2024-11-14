using UnityEngine;

public class CloudManager : MonoBehaviour
{
    [System.Serializable]
    public class Cloud
    {
        public GameObject cloudObject;  // ���� ������Ʈ (UI Image)
        public float speed = 1f;        // ���� �̵� �ӵ�
    }

    [SerializeField] private Cloud[] clouds;   // ���� �������� ������ �迭
    [SerializeField] private float screenLeftEdge = -10f;   // ���� ���
    [SerializeField] private float screenRightEdge = 10f;   // ������ ���

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
                // ������ �������� �̵�
                cloud.cloudObject.transform.Translate(Vector3.left * cloud.speed * Time.deltaTime);

                // ������ ȭ�� ���� ��踦 �Ѿ ��� �����ʿ��� �ٽ� ��Ÿ������ ����
                RectTransform rectTransform = cloud.cloudObject.GetComponent<RectTransform>();
                float cloudWidth = rectTransform.rect.width * rectTransform.lossyScale.x; // ������ ���� ȭ��� �ʺ� ���

                if (cloud.cloudObject.transform.position.x < screenLeftEdge - cloudWidth)
                {
                    Vector3 newPosition = cloud.cloudObject.transform.position;
                    newPosition.x = screenRightEdge + cloudWidth;  // �����ʿ��� �ٽ� ��Ÿ���� ��ġ ����
                    cloud.cloudObject.transform.position = newPosition;
                }
            }
        }

    }



}