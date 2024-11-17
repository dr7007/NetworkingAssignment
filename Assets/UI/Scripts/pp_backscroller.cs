using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float speed = 1f;              // 스크롤 속도
    public RectTransform image1;          // 첫 번째 구름 이미지
    public RectTransform image2;          // 두 번째 구름 이미지
    public RectTransform canvasRt = null;

    private float imageWidth;             // 이미지의 너비
    private float halfcanvasWidth;       // Canvas의 너비/2

    void Start()
    {
        // 이미지의 너비를 구함 (스케일을 고려하여 실제 크기 계산)
        imageWidth = image1.rect.width;
        halfcanvasWidth = canvasRt.sizeDelta.x / 2;

        image1.anchoredPosition = new Vector2(halfcanvasWidth, 0f);
        // 두 번째 이미지를 첫 번째 이미지의 오른쪽에 위치시킴
        image2.anchoredPosition = new Vector2(image1.anchoredPosition.x + imageWidth, 0f);
    }

    void Update()
    {
        // 두 이미지가 왼쪽으로 이동
        image1.anchoredPosition += Vector2.left * speed * Time.deltaTime;
        image2.anchoredPosition += Vector2.left * speed * Time.deltaTime;

        // 이미지가 왼쪽 화면 밖으로 완전히 나가면 오른쪽으로 재배치
        if (image1.anchoredPosition.x <= -(imageWidth / 2 + halfcanvasWidth))
        {
            image1.anchoredPosition = new Vector2(image2.anchoredPosition.x + imageWidth, image1.anchoredPosition.y);
        }
        if (image2.anchoredPosition.x <= -(imageWidth / 2 + halfcanvasWidth))
        {
            image2.anchoredPosition = new Vector2(image1.anchoredPosition.x + imageWidth, image2.anchoredPosition.y);
        }
    }
}