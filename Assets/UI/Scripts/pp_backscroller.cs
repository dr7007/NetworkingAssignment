using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float speed = 1f;              // ��ũ�� �ӵ�
    public RectTransform image1;          // ù ��° ���� �̹���
    public RectTransform image2;          // �� ��° ���� �̹���
    public RectTransform canvasRt = null;

    private float imageWidth;             // �̹����� �ʺ�
    private float halfcanvasWidth;       // Canvas�� �ʺ�/2

    void Start()
    {
        // �̹����� �ʺ� ���� (�������� ����Ͽ� ���� ũ�� ���)
        imageWidth = image1.rect.width;
        halfcanvasWidth = canvasRt.sizeDelta.x / 2;

        image1.anchoredPosition = new Vector2(halfcanvasWidth, 0f);
        // �� ��° �̹����� ù ��° �̹����� �����ʿ� ��ġ��Ŵ
        image2.anchoredPosition = new Vector2(image1.anchoredPosition.x + imageWidth, 0f);
    }

    void Update()
    {
        // �� �̹����� �������� �̵�
        image1.anchoredPosition += Vector2.left * speed * Time.deltaTime;
        image2.anchoredPosition += Vector2.left * speed * Time.deltaTime;

        // �̹����� ���� ȭ�� ������ ������ ������ ���������� ���ġ
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