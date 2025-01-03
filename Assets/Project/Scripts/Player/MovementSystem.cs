using DG.Tweening;
using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    private Rigidbody rb;
    public float rotateSpeed = 1.0f;
    public float boostSpeed = 1.0f; 
    public Animator animator;
    public float dampTime;
    private float rotateDirection;
    private float rotateDifference;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleAnimations();
        RotateTowards(LineManager.instance.GetTurnDirection());
        Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + gameObject.transform.forward * 40, Color.green);
    }


    // ROTATION HANDLING -----------------------------------------------------
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




    // MOVEMENT HANDLING -----------------------------------------------------
    public void Boost(Vector3 direction)
    {
        if (TurnBasedManager.instance.IsTimePaused())
            return; 
        rb.DOMove(direction, boostSpeed).SetEase(Ease.InOutSine);
    }





    // ANIMATION HANDLING ---------------------------------------------------
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
