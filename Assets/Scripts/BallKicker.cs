using UnityEngine;

public class BallKicker : MonoBehaviour
{
    [Header("Kick Settings")]
    [SerializeField] private float detectRadius = 3.5f;
    [SerializeField] private float kickForce = 25f;

    [Header("Camera Reference")]
    [SerializeField] private CameraController mainCameraController;

    public float DetectRadius => detectRadius;

    private void Awake()
    {
        if (mainCameraController == null && Camera.main != null)
        {
            mainCameraController = Camera.main.GetComponent<CameraController>();
        }
    }

    // Sút quả bóng gần nhất
    public void KickNearestBall()
    {
        GameObject ball = GetNearestBall();
        if (ball != null) ExecuteKick(ball);
    }

    // Auto Kick: Sút quả bóng xa nhất
    public void AutoKickFarthestBall()
    {
        GameObject[] allBalls = GameObject.FindGameObjectsWithTag("Ball");
        if (allBalls.Length == 0) return;

        GameObject farthestBall = null;
        float maxDistance = -1f;

        foreach (GameObject ball in allBalls)
        {
            float dist = Vector3.Distance(transform.position, ball.transform.position);
            if (dist > maxDistance)
            {
                maxDistance = dist;
                farthestBall = ball;
            }
        }

        if (farthestBall != null) ExecuteKick(farthestBall);
    }

    private void ExecuteKick(GameObject ball)
    {
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        if (ballRb == null) return;

        // Tìm khung thành gần quả bóng nhất
        GameObject nearestGoal = GetNearestGoal(ball.transform.position);

        Vector3 kickDirection = nearestGoal != null
            ? (nearestGoal.transform.position - ball.transform.position).normalized
            : transform.forward;

        ballRb.isKinematic = false;
        ballRb.linearVelocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;

        Vector3 force = (kickDirection + Vector3.up * 0.25f).normalized * kickForce;
        ballRb.AddForce(force, ForceMode.Impulse);

        // Quay nhân vật về hướng sút
        Vector3 lookDir = new Vector3(kickDirection.x, 0, kickDirection.z);
        if (lookDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        // Gọi Camera đuổi theo bóng
        if (mainCameraController != null)
        {
            mainCameraController.FollowBall(ball.transform);
        }
    }

    // Tìm khung thành gần bóng nhất
    private GameObject GetNearestGoal(Vector3 ballPosition)
    {
        GameObject[] allGoals = GameObject.FindGameObjectsWithTag("Goal");
        if (allGoals.Length == 0) return null;

        GameObject nearestGoal = null;
        float minDistance = float.MaxValue;

        foreach (GameObject goal in allGoals)
        {
            float dist = Vector3.Distance(ballPosition, goal.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearestGoal = goal;
            }
        }

        return nearestGoal;
    }

    // Tìm quả bóng gần nhân vật nhất
    public GameObject GetNearestBall()
    {
        GameObject[] allBalls = GameObject.FindGameObjectsWithTag("Ball");
        if (allBalls.Length == 0)
        {
            return GameObject.Find("Soccer Ball");
        }

        GameObject nearestBall = null;
        float minDistance = float.MaxValue;

        foreach (GameObject ball in allBalls)
        {
            float dist = Vector3.Distance(transform.position, ball.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearestBall = ball;
            }
        }

        return nearestBall;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}