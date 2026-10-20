using UnityEngine;

/// Drives the owl Animator from ability/physics state:
/// isFlying = flight ability active, isMoving = measured horizontal movement.
/// Sprite flipping stays in Owl.Movement (the art faces left; flipX = true means facing right).
[RequireComponent(typeof(Animator))]
public class OwlAnimationDriver : MonoBehaviour
{
    public float walkThreshold = 0.05f;

    Animator animator;
    Owl owl;
    float lastFixedX;
    bool measuredMoving;

    void Awake()
    {
        animator = GetComponent<Animator>();
        owl = GetComponentInParent<Owl>();
        lastFixedX = transform.position.x;
    }

    // measured on the physics clock, like BearAnimationDriver
    void FixedUpdate()
    {
        float measuredVx = (transform.position.x - lastFixedX) / Time.fixedDeltaTime;
        lastFixedX = transform.position.x;
        measuredMoving = Mathf.Abs(measuredVx) > walkThreshold;
    }

    void Update()
    {
        animator.SetBool("isFlying", owl != null && owl.IsFlying);
        animator.SetBool("isMoving", measuredMoving);
    }
}
