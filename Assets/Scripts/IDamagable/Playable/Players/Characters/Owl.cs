using UnityEngine;

public class Owl : PlayableCharacter
{
    // Added flight special ability for the bird character (17.10.26, Ola)

    //[SerializeField] public float jumpForce = 8f;
    [SerializeField] public Rigidbody2D rb;
    private SpriteRenderer sprite;
    
    [Header("Flight Ability")]
    [SerializeField] private float flightSpeed = 5f;     // how fast the bird flies
    [SerializeField] private float maxFlightTime = 5f;   // seconds the bird can stay in the air
    [SerializeField] private float flightCooldown = 1.5f;  // seconds before it can fly again (stops Space-spam infinite lift)
    [SerializeField] private float liftSpeed = 4f;
    [SerializeField] private float liftDuration = 0.5f;
    private float liftTimeLeft;
    [SerializeField] private float flightAcceleration = 25f;
    [SerializeField] private float flightDeceleration = 20f;


    [SerializeField] private LayerMask takeoffGroundMask = (1 << 8) | (1 << 6) | 1; // Ground, Boulder, Default

    private bool isFlying = false; // true while the ability is active
    private float flightTimeLeft; // countdown while flying
    private float cooldownTimeLeft; // countdown after landing
    private float normalGravity; // the gravity we restore after flying
    private Collider2D bodyCollider;

    public bool IsFlying => isFlying; // read by OwlAnimationDriver

    void Awake()
    {
        normalGravity = rb.gravityScale;
        sprite = GetComponentInChildren<SpriteRenderer>();
        bodyCollider = GetComponent<Collider2D>();
    }

    // flight may only START from the ground; re-triggering lift mid-air allowed
    // climbing forever and flying over the whole level
    private bool IsGrounded()
    {
        if (bodyCollider == null)
            return true;
        Vector2 feet = new Vector2(bodyCollider.bounds.center.x, bodyCollider.bounds.min.y + 0.05f);
        return Physics2D.Raycast(feet, Vector2.down, 0.35f, takeoffGroundMask);
    }

    void Update()
    {
        // count the cooldown down until it reaches 0
        if (cooldownTimeLeft > 0f)
        {
            cooldownTimeLeft -= Time.deltaTime;
        }

        // while flying, count the flight time down
        if (isFlying)
        {
            flightTimeLeft -= Time.deltaTime;

            if (flightTimeLeft <= 0f)
            {
                StopFlying();
            }
        }
        if (liftTimeLeft > 0f)
        {
            liftTimeLeft -= Time.deltaTime;
        }
    }

    // Called by CharacterMovement when Space is pressed (via PlayableCharacter).
    public override void SpecialAbility()
    {
        if (isFlying)
        {
            StopFlying(); // second press = land early
        }
        else if (cooldownTimeLeft <= 0f && IsGrounded())
        {
            StartFlying(); // only from the ground, and only once the cooldown is over
        }
    }

    public override void Movement(Rigidbody2D rb, float horizontalInput, float verticalInput)
    {
        if (horizontalInput > 0.1f)
        {
            sprite.flipX = true;    // walking right: flipped, so the owl faces right
        }
        else if (horizontalInput < -0.1f)
        {
            sprite.flipX = false;   // walking left: the original drawing faces left
        }
        if (isFlying)
        {
                Vector2 input = new Vector2(horizontalInput, 0f);   // up/down keys ignored

                // the speed we WANT to reach
                Vector2 targetVelocity = input.normalized * flightSpeed;

                // is just took off
                if (liftTimeLeft > 0f)
                {
                    targetVelocity.y = liftSpeed;
                }
                else
                {
                    targetVelocity.y = rb.linearVelocity.y;
                }

                // speed up when pressing keys or taking off; slow down otherwise
                float rate = flightDeceleration;
                if (input.magnitude > 0.01f || liftTimeLeft > 0f)
                {
                    rate = flightAcceleration;
                }

                rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, rate * Time.deltaTime);

                if (rb.linearVelocity.y < -1.5f)
{
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -1.5f);
}

            //old flight, deleted (19.10.26, Ola)
            // Vector2 direction = new Vector2(horizontalInput, verticalInput).normalized;
            // Vector2 velocity = direction * flightSpeed;

            // if (liftTimeLeft > 0f)
            // {
            //     velocity.y = liftSpeed;   // during the lift, always rise, ignoring up/down keys
            // }

            // rb.linearVelocity = velocity;
        }
        else
        {
            // not flying: walk like the other characters
            base.Movement(rb, horizontalInput, verticalInput);
        }
    }

    private void StartFlying()
    {
        isFlying = true;
        liftTimeLeft = liftDuration;
        flightTimeLeft = maxFlightTime;
        rb.gravityScale = 0.3f;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
    }

    private void StopFlying()
    {
        isFlying = false;
        cooldownTimeLeft = flightCooldown;     // start the cooldown
        rb.gravityScale = normalGravity;       // gravity back on, the bird falls
    }

    // Unity calls this when the bird is switched off (switching characters).
    void OnDisable()
    {
        if (isFlying)
        {
            StopFlying();
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
  

    
}
