using UnityEngine;

/// Drives the hedgehog Animator from ability/physics state:
/// isRolling = curl ability active, isWalking = measured horizontal movement.
/// Sprite flipping stays in Hedgehog.Movement; the roll mechanic scales the transform,
/// so the clips must never key localScale.
[RequireComponent(typeof(Animator))]
public class HedgehogAnimationDriver : MonoBehaviour
{
    public float walkThreshold = 0.05f;

    Animator animator;
    Hedgehog hedgehog;
    float lastFixedX;
    bool measuredMoving;

    void Awake()
    {
        animator = GetComponent<Animator>();
        hedgehog = GetComponentInParent<Hedgehog>();
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
        bool rolling = hedgehog != null && hedgehog.IsRolling;
        animator.SetBool("isRolling", rolling);
        animator.SetBool("isWalking", measuredMoving && !rolling);
    }
}
