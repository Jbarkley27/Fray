using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRotationModule : MonoBehaviour
{
    public Rigidbody rb;
    public float rotateSpeed = 1.0f;
    private NavMeshAgent mNavMeshAgent;

    // public float attackRange = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        mNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowards(TurnBasedManager.instance.movementSystem.gameObject.transform.position - transform.position);

        // if (IsWithinRange() || TurnBasedManager.instance.IsTimePaused())
        // {
        //     Debug.Log("Stopped");
        //     mNavMeshAgent.isStopped = true;
        // }
        // else 
        // {
        //     mNavMeshAgent.isStopped = false;
        //     MoveToPlayer();
        // }

        // MoveToPlayer();

        Debug.Log("In Range: " + IsWithinRange());
    }

    public void RotateTowards(Vector3 targetDirection)
    {
        if (TurnBasedManager.instance.IsTimePaused())
        {
            return;
        }

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Smoothly interpolate between current and target rotation
        Quaternion smoothedRotation = Quaternion.Slerp(
            rb.rotation,             // Current rotation
            targetRotation,          // Target rotation
            rotateSpeed * Time.deltaTime // Interpolation factor
        );

        // Apply the smooth rotation to the Rigidbody
        rb.MoveRotation(smoothedRotation);
    }

    public bool IsWithinRange()
    {
        // // Check if we've reached the destination
        // if (!mNavMeshAgent.pathPending)
        // {
        //     if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance)
        //     {
        //         if (!mNavMeshAgent.hasPath || mNavMeshAgent.velocity.sqrMagnitude == 0f)
        //         {
        //             // Done
        //             return true;
        //         }
        //     }
        // }

        // return false;

        return Vector3.Distance(transform.position, TurnBasedManager.instance.movementSystem.gameObject.transform.position) < mNavMeshAgent.stoppingDistance;
    }

    public void MoveToPlayer()
    {
        ResumeMovement();
        mNavMeshAgent.SetDestination(TurnBasedManager.instance.movementSystem.gameObject.transform.position);
    }

    public void StopMovement()
    {
        mNavMeshAgent.isStopped = true;
    }

    public void ResumeMovement()
    {
        mNavMeshAgent.isStopped = false;
    }
}
