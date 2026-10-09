using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button kickButton;
    [SerializeField] private Button autoKickButton;
    [SerializeField] private Button resetButton;

    [Header("References")]
    [SerializeField] private BallKicker ballKicker;

    private void Start()
    {
        if (ballKicker == null)
        {
            ballKicker = FindAnyObjectByType<BallKicker>();
        }

        if (kickButton != null && ballKicker != null)
        {
            kickButton.onClick.AddListener(ballKicker.KickNearestBall);
        }

        if (autoKickButton != null && ballKicker != null)
        {
            autoKickButton.onClick.AddListener(ballKicker.AutoKickFarthestBall);
        }

        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetScene);
        }
    }

    private void Update()
    {
        CheckKickButtonVisibility();
    }

    private void CheckKickButtonVisibility()
    {
        if (kickButton == null || ballKicker == null) return;

        GameObject ball = ballKicker.GetNearestBall();

        if (ball != null)
        {
            float distance = Vector3.Distance(ballKicker.transform.position, ball.transform.position);
            kickButton.gameObject.SetActive(distance <= ballKicker.DetectRadius);
        }
        else
        {
            kickButton.gameObject.SetActive(false);
        }
    }

    public void ResetScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}