using UnityEngine;

public class pp_cloud : MonoBehaviour
{
    public float speed = 10f; // 구름 이동 속도
    
    void Update()
    {
        // 오른쪽으로 이동
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // 오른쪽 끝으로 이동하면 화면 왼쪽으로 되돌아가기
        if (transform.position.x > 380f) // 오른쪽 끝을 넘으면
        {
            transform.position = new Vector3(-10f, transform.position.y, transform.position.z); // 화면 왼쪽에서 다시 시작
        }
    }
}