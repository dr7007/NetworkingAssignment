using UnityEngine;

public class pp_bs_vertical : MonoBehaviour
{
    public float speed = 1f;              // ��ũ�� �ӵ�
    public RectTransform image1;          // ù ��° �̹���
    public RectTransform image2;          // �� ��° �̹���
    public RectTransform canvasRt = null;

    private float imageHeight;             // �̹����� ����
    private float halfcanvasHeight;        // Canvas�� ����/2

    void Start()
    {
        // �̹����� ���̸� ���� (�������� ����Ͽ� ���� ũ�� ���)
        imageHeight = image1.rect.height;
        halfcanvasHeight = canvasRt.sizeDelta.y/2;

        image1.anchoredPosition = new Vector2(0f, -(imageHeight/2 + halfcanvasHeight));
        // �� ��° �̹����� ù ��° �̹����� �Ʒ��ʿ� ��ġ��Ŵ ?
        image2.anchoredPosition = new Vector2(0f, image1.anchoredPosition.y - imageHeight);
    }

    void Update()
    {
        // �� �̹����� �������� �̵�
        image1.anchoredPosition += Vector2.up * speed * Time.deltaTime;
        image2.anchoredPosition += Vector2.up * speed * Time.deltaTime;

        // �̹����� ���� ȭ�� ������ ������ ������ ���������� ���ġ
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



