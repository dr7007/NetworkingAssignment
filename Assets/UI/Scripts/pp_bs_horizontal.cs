using UnityEngine;

public class pp_bs_horizontal : MonoBehaviour
{
    public float speed = 1f;              // ��ũ�� �ӵ�
    public RectTransform image1;          // ù ��° �̹���
    public RectTransform image2;          // �� ��° �̹���

    private float imageHeight;             // �̹����� ����

    void Start()
    {
        // �̹����� ���̸� ���� (�������� ����Ͽ� ���� ũ�� ���)
        imageHeight = image1.rect.height * image1.lossyScale.y;

        // �� ��° �̹����� ù ��° �̹����� �Ʒ��ʿ� ��ġ��Ŵ ?
        image2.anchoredPosition = new Vector2(image2.anchoredPosition.x, -imageHeight);
    }

    void Update()
    {
        // �� �̹����� �������� �̵�
        image1.anchoredPosition += Vector2.up * speed * Time.deltaTime;
        image2.anchoredPosition += Vector2.up * speed * Time.deltaTime;

        // �̹����� ���� ȭ�� ������ ������ ������ ���������� ���ġ
        if (image1.anchoredPosition.y >= imageHeight)
        {
            image1.anchoredPosition = new Vector2(image1.anchoredPosition.x, image2.anchoredPosition.y - imageHeight);
        }
        if (image2.anchoredPosition.y >= imageHeight)
        {
            image2.anchoredPosition = new Vector2(image2.anchoredPosition.x, image1.anchoredPosition.y - imageHeight);
        }
    }
}



