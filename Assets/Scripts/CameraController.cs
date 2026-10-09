using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private Transform playerTarget;
    private Transform currentTarget;

    [Header("Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 12, -8);
    [SerializeField] private float smoothSpeed = 5f;

    private Coroutine followBallRoutine;

    private void Start()
    {
        currentTarget = playerTarget;
    }

    private void LateUpdate()
    {
        if (currentTarget == null) return;

        // Cập nhật vị trí Camera mượt mà theo target hiện tại
        Vector3 targetPosition = currentTarget.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

    // Gọi hàm này trong BallKicker khi sút bóng
    public void FollowBall(Transform ballTransform)
    {
        if (ballTransform == null) return;

        if (followBallRoutine != null)
        {
            StopCoroutine(followBallRoutine);
        }

        followBallRoutine = StartCoroutine(FollowBallProcess(ballTransform));
    }

    private IEnumerator FollowBallProcess(Transform ballTransform)
    {
        // 1. Chuyển Camera đi theo quả bóng
        currentTarget = ballTransform;

        Rigidbody ballRb = ballTransform.GetComponent<Rigidbody>();

        // Chờ 0.3s để bóng thực sự cất cánh sau cú sút
        yield return new WaitForSeconds(0.3f);

        // 2. Theo dõi quả bóng cho đến khi nó dừng lại hoặc vận tốc rất nhỏ (sau khi chạm khung thành/lưới)
        if (ballRb != null)
        {
            while (ballRb.linearVelocity.magnitude > 0.5f)
            {
                yield return null; 
            }
        }

        yield return new WaitForSeconds(2.0f);

        // 4. Chuyển Camera quay trở lại nhân vật
        currentTarget = playerTarget;
        followBallRoutine = null;
    }
    
    // Khi bóng chạm khung thành
    public void OnBallHitGoal()
    {
        if (followBallRoutine != null)
        {
            StopCoroutine(followBallRoutine);
        }
        followBallRoutine = StartCoroutine(ReturnToPlayerAfterDelay(2f));
    }

    private IEnumerator ReturnToPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentTarget = playerTarget;
        followBallRoutine = null;
    }
}