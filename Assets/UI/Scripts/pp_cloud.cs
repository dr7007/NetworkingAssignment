using UnityEngine;

public class pp_cloud : MonoBehaviour
{
    public float speed = 10f; // ���� �̵� �ӵ�
    
    void Update()
    {
        // ���������� �̵�
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // ������ ������ �̵��ϸ� ȭ�� �������� �ǵ��ư���
        if (transform.position.x > 380f) // ������ ���� ������
        {
            transform.position = new Vector3(-10f, transform.position.y, transform.position.z); // ȭ�� ���ʿ��� �ٽ� ����
        }
    }
}