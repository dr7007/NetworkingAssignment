using UnityEngine;

public class pp_bs_vertical : MonoBehaviour
{
    public float speed = 1f;              // 스크롤 속도
    public RectTransform image1;          // 첫 번째 이미지
    public RectTransform image2;          // 두 번째 이미지
    public RectTransform canvasRt = null;

    private float imageHeight;             // 이미지의 높이
    private float halfcanvasHeight;        // Canvas의 높이/2

    void Start()
    {
        // 이미지의 높이를 구함 (스케일을 고려하여 실제 크기 계산)
        imageHeight = image1.rect.height;
        halfcanvasHeight = canvasRt.sizeDelta.y/2;

        image1.anchoredPosition = new Vector2(0f, -(imageHeight/2 + halfcanvasHeight));
        // 두 번째 이미지를 첫 번째 이미지의 아래쪽에 위치시킴 ?
        image2.anchoredPosition = new Vector2(0f, image1.anchoredPosition.y - imageHeight);
    }

    void Update()
    {
        // 두 이미지가 위쪽으로 이동
        image1.anchoredPosition += Vector2.up * speed * Time.deltaTime;
        image2.anchoredPosition += Vector2.up * speed * Time.deltaTime;

        // 이미지가 위쪽 화면 밖으로 완전히 나가면 오른쪽으로 재배치
        if (image1.anchoredPosition.y >= (imageHeight/2 + halfcanvasHeight))
        {
            image1.anchoredPosition = new Vector2(image1.anchoredPosition.x, image2.anchoredPosition.y - imageHeight);
        }
        if (image2.anchoredPosition.y >= (imageHeight/2 + halfcanvasHeight))
        {
            image2.anchoredPosition = new Vector2(image2.anchoredPosition.x, image1.anchoredPosition.y - imageHeight);
        }
    }
}



