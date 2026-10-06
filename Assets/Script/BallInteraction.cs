using UnityEngine;

public class BallInteraction : MonoBehaviour
{
    [Header("Kick Settings")]
    [SerializeField] private float kickDistance;
    [SerializeField] private float kickForce;
    [SerializeField] private float kickHeight;

    [Header("UI")]
    [SerializeField] private GameObject kickBtn;
    [SerializeField] private GameObject autoBtn;

    private GameObject nearestBall;
    private GameObject nearestGoal;

    private TopDownCamera topDownCamera;

    private void Start()
    {
        if (kickBtn != null) kickBtn.SetActive(false);
        topDownCamera = FindObjectOfType<TopDownCamera>();
    }

    private void Update()
    {
        FindNearestBall();
        if (nearestBall == null)
        {
            HidekickBtn();
            return;
        }

        float distance = Vector3.Distance(transform.position, nearestBall.transform.position);
        bool canKick = distance <= kickDistance;
        if (kickBtn != null) kickBtn.SetActive(canKick);
    }

    private void FindNearestBall()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("SoccerBall");
        float closestDistance = Mathf.Infinity;
        nearestBall = null;

        foreach (GameObject ball in balls)
        {
            float distance = Vector3.Distance(transform.position, ball.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestBall = ball;
            }
        }
    }

    public void KickBall()
    {
        if (nearestBall == null) return;
        float distance = Vector3.Distance(transform.position, nearestBall.transform.position);
        if (distance > kickDistance) return;
        FindNearestGoal();

        if (nearestGoal == null) return;
        Rigidbody ballRigidbody = nearestBall.GetComponent<Rigidbody>();

        if (ballRigidbody == null) return;

        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;

        Vector3 targetPosition = nearestGoal.transform.position;
        targetPosition.y += kickHeight;
        Vector3 direction = (targetPosition - nearestBall.transform.position).normalized;
        ballRigidbody.AddForce(direction * kickForce, ForceMode.Impulse);

        if(topDownCamera != null) topDownCamera.FollowBall(nearestBall.transform);
        if (kickBtn != null) kickBtn.SetActive(false);
    }

    private void FindNearestGoal()
    {
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");
        float closestDistance = Mathf.Infinity;
        nearestGoal = null;

        foreach (GameObject goal in goals)
        {
            float distance = Vector3.Distance(transform.position,goal.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestGoal = goal;
            }
        }
    }

    public void AutoKick()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("SoccerBall");
        if (balls.Length == 0) return;
        GameObject farthestBall = null;
        float farthestDistance = -Mathf.Infinity;

        foreach (GameObject ball in balls)
        {
            float distance = Vector3.Distance(transform.position,ball.transform.position);
            if (distance > farthestDistance)
            {
                farthestDistance = distance;
                farthestBall = ball;
            }
        }

        if (farthestBall == null) return;
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");
        if (goals.Length == 0) return;

        GameObject closestGoal = null;
        float closestGoalDistance = Mathf.Infinity;

        foreach (GameObject goal in goals)
        {
            float distance = Vector3.Distance(farthestBall.transform.position, goal.transform.position);
            if (distance < closestGoalDistance)
            {
                closestGoalDistance = distance;
                closestGoal = goal;
            }
        }

        if (closestGoal == null) return;
        Rigidbody ballRigidbody = farthestBall.GetComponent<Rigidbody>();
        if (ballRigidbody == null) return;

        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;

        Vector3 targetPosition = closestGoal.transform.position;
        targetPosition.y += kickHeight;
        Vector3 direction = (targetPosition - farthestBall.transform.position).normalized;
        ballRigidbody.AddForce(direction * kickForce, ForceMode.Impulse);

        if (topDownCamera != null) topDownCamera.FollowBall(farthestBall.transform);
    }

    private void HidekickBtn()
    {
        if (kickBtn != null) kickBtn.SetActive(false);
    }
}