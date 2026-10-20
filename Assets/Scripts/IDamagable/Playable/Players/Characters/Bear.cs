using UnityEngine;
using UnityEngine.InputSystem;

public class Bear : PlayableCharacter
{
    [SerializeField] public float acceleration = 7f;   // units/sec^2 while speeding up - low value = heavy bear
    [SerializeField] public float deceleration = 10f;  // units/sec^2 while stopping

    [Header("Boulder Pushing (hold SPACE)")]
    [SerializeField] private float pushReach = 0.7f;            // how far in front the bear can reach a boulder
    [SerializeField] private LayerMask boulderMask = 1 << 6;    // Boulder layer
    [SerializeField] private Rigidbody2D rb;

    private SpriteRenderer sprite;
    private Collider2D bodyCollider;
    private bool pushHeld;

    public bool IsShoving { get; private set; } // read by BearAnimationDriver for the push animation

    private void Awake()
    {
        if (rb == null) // make sure we have rb
        {
            rb = GetComponent<Rigidbody2D>();
        }
        sprite = GetComponentInChildren<SpriteRenderer>();
        bodyCollider = GetComponent<Collider2D>();
    }

    public override void Movement(Rigidbody2D body, float horizontalInput, float verticalInput)
        {
            // Preserve the Bear's heavy acceleration and deceleration.
            float targetVx = horizontalInput * moveSpeed;
            float rate = Mathf.Abs(targetVx) > 0.01f ? acceleration : deceleration;
            float vx = Mathf.MoveTowards(body.linearVelocity.x, targetVx, rate * Time.fixedDeltaTime);

            body.linearVelocity = new Vector2(vx, body.linearVelocity.y);
        }

    // SPACE held = keep pushing the boulder in front; released = stop.
    public override void SpecialAbility()
    {
        pushHeld = true;
    }

    public override void SpecialAbilityReleased()
    {
        pushHeld = false;
    }

    void FixedUpdate()
    {
        bool pushing = false;

        if (pushHeld && !IsKnockedBack)
        {
            // bear art faces right; flipX means facing left (same convention as BearAnimationDriver)
            float direction = (sprite != null && sprite.flipX) ? -1f : 1f;

            // overlap zone just in front of the bear (OverlapBox also catches a boulder
            // the bear is already leaning against, which a BoxCast can skip)
            Bounds b = bodyCollider.bounds;
            Vector2 zoneCenter = (Vector2)b.center + new Vector2(direction * (b.extents.x + pushReach * 0.5f), 0f);
            Vector2 zoneSize = new Vector2(pushReach + 0.3f, b.size.y * 0.8f);
            Collider2D hit = Physics2D.OverlapBox(zoneCenter, zoneSize, 0f, boulderMask);

            if (hit != null)
            {
                PushableBoulder boulder = hit.GetComponentInParent<PushableBoulder>();
                if (boulder != null)
                    pushing = boulder.PushTick(direction);
            }
        }

        IsShoving = pushing;
    }

    void OnDisable()
    {
        pushHeld = false;
        IsShoving = false;
    }
}
