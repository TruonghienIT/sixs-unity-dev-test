using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JammoMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;

    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3 (x, 0f, y).normalized;
        bool isRun = direction.magnitude > 0.1f;
        animator.SetBool("isRun", isRun);

        if (isRun)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed*Time.deltaTime);
        }
    }
}
