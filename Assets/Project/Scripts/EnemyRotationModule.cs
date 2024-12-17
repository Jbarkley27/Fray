using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotationModule : MonoBehaviour
{
    public Rigidbody rb;
    public float rotateSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        RotateTowards(TurnBasedManager.instance.movementSystem.gameObject.transform.position - transform.position);
    }

    public void RotateTowards(Vector3 targetDirection)
    {
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
}
