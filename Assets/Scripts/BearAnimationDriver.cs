using UnityEngine;

/// Drives the bear Animator from physics state:
/// isWalking = horizontal movement, isPushing = walking while touching the boulder.
/// Also flips the sprite to face the move direction.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class BearAnimationDriver : MonoBehaviour
{
    public float walkThreshold = 0.05f;
    public float pushCheckDistance = 0.25f;
    public LayerMask boulderMask = 1 << 6; // Boulder layer

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sprite;
    Collider2D bodyCollider;
    int facing = 1; // 1 = right (art faces right), -1 = left
    float lastX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        bodyCollider = GetComponent<Collider2D>();
        lastX = transform.position.x;
    }

    void Update()
    {
        // measured movement, not input velocity, so pressing against a wall reads as standing still
        float measuredVx = Time.deltaTime > 0f ? (transform.position.x - lastX) / Time.deltaTime : 0f;
        lastX = transform.position.x;
        bool moving = Mathf.Abs(measuredVx) > walkThreshold;

        float inputVx = rb.linearVelocity.x;
        if (Mathf.Abs(inputVx) > walkThreshold)
        {
            facing = inputVx > 0f ? 1 : -1;
            sprite.flipX = facing < 0;
        }

        bool pushing = moving && TouchingBoulderAhead();

        animator.SetBool("isWalking", moving && !pushing);
        animator.SetBool("isPushing", pushing);
    }

    bool TouchingBoulderAhead()
    {
        if (bodyCollider == null) return false;
        Bounds b = bodyCollider.bounds;
        Vector2 origin = new Vector2(b.center.x, b.center.y);
        Vector2 size = new Vector2(b.size.x, b.size.y * 0.8f);
        RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0f, new Vector2(facing, 0f), pushCheckDistance, boulderMask);
        return hit.collider != null && hit.collider.CompareTag("Boulder");
    }
}
