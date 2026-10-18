using UnityEngine;

public class owl : PlayableCharacter
{
    // Added flight special ability for the bird character (17.10.26, Ola)

    //[SerializeField] public float jumpForce = 8f;
    [SerializeField] public Rigidbody2D rb;
    private SpriteRenderer sprite;
    
    [Header("Flight Ability")]
    [SerializeField] private float flightSpeed = 5f;     // how fast the bird flies
    [SerializeField] private float maxFlightTime = 5f;   // seconds the bird can stay in the air
    [SerializeField] private float flightCooldown = 4f;  // seconds before it can fly again 
    [SerializeField] private float liftSpeed = 4f;
    [SerializeField] private float liftDuration = 0.25f;
    private float liftTimeLeft;

    private bool isFlying = false; // true while the ability is active
    private float flightTimeLeft; // countdown while flying
    private float cooldownTimeLeft; // countdown after landing
    private float normalGravity; // the gravity we restore after flying

    void Awake()
    {
        normalGravity = rb.gravityScale;
        sprite = GetComponentInChildren<SpriteRenderer>();
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
        else if (cooldownTimeLeft <= 0f)
        {
            StartFlying(); // only start if the cooldown is over
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
            Vector2 direction = new Vector2(horizontalInput, verticalInput).normalized;
            Vector2 velocity = direction * flightSpeed;

            if (liftTimeLeft > 0f)
            {
                velocity.y = liftSpeed;   // during the lift, always rise, ignoring up/down keys
            }

            rb.linearVelocity = velocity;
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
        rb.gravityScale = 0f;                  // no gravity, so the bird floats
        rb.linearVelocity = Vector2.zero;      // clear any falling speed
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
