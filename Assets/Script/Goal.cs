using System.Collections;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [Header("Goal Effect")]
    [SerializeField] private ParticleSystem confettiEffect;

    [Header("Camera")]
    [SerializeField] private TopDownCamera topDownCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SoccerBall"))
        {
            if (confettiEffect != null) confettiEffect.Play();
            if (topDownCamera != null) topDownCamera.BallReachedGoal();
            StartCoroutine(DeleteBallAfterDelay(other.gameObject));
        }
    }

    private IEnumerator DeleteBallAfterDelay(GameObject ball)
    {
        yield return new WaitForSeconds(2f);
        if (ball != null) Destroy(ball);
    }
}