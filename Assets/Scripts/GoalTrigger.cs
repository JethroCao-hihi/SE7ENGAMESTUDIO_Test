using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [Header("Confetti Effect")]
    [SerializeField] private ParticleSystem confettiParticle;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball") || collision.gameObject.name.ToLower().Contains("ball") || collision.gameObject.name.ToLower().Contains("soccer"))
        {
            if (confettiParticle != null)
            {
                // Điểm va chạm đầu tiên giữa bóng và khung thành
                ContactPoint contact = collision.contacts[0];
                Vector3 hitPoint = contact.point;
                Vector3 hitNormal = contact.normal;

                // Particle System đến đúng vị trí điểm va chạm
                confettiParticle.transform.position = hitPoint;

                // Particle hướng ra ngoài theo bề mặt va chạm
                confettiParticle.transform.rotation = Quaternion.LookRotation(hitNormal);

                // Hiệu ứng được phát lại
                confettiParticle.Stop();
                confettiParticle.Play();

                Debug.Log("Goallllll!!!!!!");
            }
            else
            {
                Debug.LogWarning("Chưa gán effect vào");
            }
        }
    }
}