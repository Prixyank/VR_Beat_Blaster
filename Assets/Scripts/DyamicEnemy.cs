using UnityEngine;
using System.Collections.Generic;

public class DyamicEnemy : MonoBehaviour
{
    public enum EnemyState { Idle, Moving, Attacking }
    public EnemyState currentState = EnemyState.Idle;

    public float detectionRange = 10f;
    public List<Transform> pathPoints;
    private int pathIndex = 0;

    public Transform player;
    public float speed = 3f;
    
    public SimpleShoot shooter;
    private bool playerInRange = false;
    private Vector3 Delta;
    private void Start()
    {
        player = Camera.main.transform; // Assuming the player is the main camera for this example
        shooter.enabled = false; // Disable shooting initially
         // Calculate the offset between shooter and enemy
    }
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange && currentState == EnemyState.Idle)
        {
            currentState = EnemyState.Moving;
        }

        switch (currentState)
        {
            case EnemyState.Moving:
                FollowPath();
                break;

            case EnemyState.Attacking:
                AimAndShoot();
                break;
        }
    }

    void FollowPath()
    {
        if (pathIndex >= pathPoints.Count)
        {
            currentState = EnemyState.Attacking;
            return;
        }
        
        Vector3 targetPoint = pathPoints[pathIndex].position; // Offset by shooter positi
        //targetPoint.position -= Delta; // Adjust target point to be relative to shooter
        targetPoint = new Vector3(targetPoint.x, transform.position.y, targetPoint.z); // Keep y level constant
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);
        transform.LookAt(targetPoint);

        if (Vector3.Distance(transform.position, targetPoint) < 0.2f)
        {
            pathIndex++;
        }
    }

    void AimAndShoot()
    {
        shooter.enabled = true; // Enable shooting
    }

    // Optional: Visualize detection range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
