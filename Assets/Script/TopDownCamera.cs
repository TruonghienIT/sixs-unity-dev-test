using System.Collections;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform Jammo;

    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 12f, -6f);
    [SerializeField] private float followSpeed;

    [Header("Goal Setting")]
    [SerializeField] private float returnDelay;

    private Transform currentTarget;
    private Coroutine returnCoroutine;

    private void Start()
    {
        currentTarget = Jammo;
    }

    private void LateUpdate()
    {
        if (currentTarget == null) return;

        Vector3 targetPosition = currentTarget.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        transform.LookAt(currentTarget);
    }

    public void FollowBall(Transform ball)
    {
        if(ball == null) return;

        if(returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }
        currentTarget = ball;
    }

    public void BallReachedGoal()
    {
        if(returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
        }
        returnCoroutine = StartCoroutine(ReturnToPlayer());
    }

    private IEnumerator ReturnToPlayer()
    {
        yield return new WaitForSeconds(returnDelay);
        currentTarget = Jammo;
        returnCoroutine = null;
    }
}