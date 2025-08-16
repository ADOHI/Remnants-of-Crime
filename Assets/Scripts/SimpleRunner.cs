using UnityEngine;

public class SimpleRunner : MonoBehaviour
{
    public float speed = 20f;
    private bool isRunning = false; // 처음엔 false

    void Update()
    {
        if (isRunning)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }

    // 👉 Timeline에서 Signal로 호출할 함수
    public void StartRunning()
    {
        isRunning = true;
    }

    public void StopRunning()
    {
        isRunning = false;
    }
}
