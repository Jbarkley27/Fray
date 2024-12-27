using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    public LineRenderer velocityLine;
    private Rigidbody rb;
    public float rotateSpeed = 1.0f;
    [Range(0f, 1f)]
    public float boostSpeed = 1.0f; 
    public Animator animator;
    public float dampTime;
    public float rotateDirection;
    public float rotateDifference;

    void Start()
    {
        // Get the Rigidbody attached to the ship
        rb = GetComponent<Rigidbody>();

        // Ensure a LineRenderer is assigned
        if (velocityLine == null)
        {
            Debug.LogError("LineRenderer is not assigned. Please attach one in the inspector.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        DrawVelocityVector();
        HandleAnimations();
        RotateTowards(LineManager.instance.GetTurnDirection());
    }

    // Draw the velocity vector using the LineRenderer
    private void DrawVelocityVector()
    {
        if (rb == null || velocityLine == null)
            return;
    }

    private void RotateTowards(Vector3 targetDirection)
    {
        if (targetDirection == Vector3.zero)
            return;

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Smoothly interpolate between current and target rotation
        Quaternion smoothedRotation = Quaternion.Slerp(
            rb.rotation,             // Current rotation
            targetRotation,          // Target rotation
            rotateSpeed * Time.deltaTime // Interpolation factor
        );

        rotateDifference = Quaternion.Angle(gameObject.transform.rotation, targetRotation);

        // find out if its rotating left or right based on the sign
        rotateDirection = Vector3.Dot(targetDirection, transform.right);

        // Apply the smooth rotation to the Rigidbody
        rb.MoveRotation(smoothedRotation);
    }

    public void Boost(Vector3 direction)
    {
        if (TurnBasedManager.instance.IsTimePaused())
            return; 
        rb.DOMove(direction, boostSpeed).SetEase(Ease.InOutSine);
    }

    public void FreezeMovement()
    {
        if (rb == null)
            return;
    }

    public void UnfreezeMovement()
    {
        if (rb == null)
            return;
    }

    public void HandleAnimations()
    {
        if (animator == null || !TurnBasedManager.instance.IsTimePaused())
            return;

        float finalRollAmount = rotateDifference;

        if (rotateDirection < 0)
        {
            finalRollAmount *= -1;
        }

        // Set the speed parameter in the animator
        animator.SetFloat("RollAmount", ScaleValue(finalRollAmount), dampTime, Time.deltaTime);
    }

    public float ScaleValue(float value)
    {
        float min = -45f;
        float max = 45f;

        // Ensure the value is clamped within the original range
        value = Mathf.Clamp(value, min, max);

        // Scale the value to the range -1 to 1
        return value / max; // Equivalent to (value - min) / (max - min) * 2 - 1
    }
}
