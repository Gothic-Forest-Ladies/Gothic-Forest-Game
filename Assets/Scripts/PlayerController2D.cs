using UnityEngine;
using UnityEngine.InputSystem;

/// Shared movement controller for all playable characters.
/// Derives from the team's PlayableCharacter so damage/death flow
/// through the existing IDamagable architecture.
///
/// IMPORTANT: do NOT declare Start() here. The base class has a private
/// Start() that initializes currentHp; declaring one here would silently
/// skip that (Unity calls only the most-derived magic method).
[RequireComponent(typeof(Rigidbody2D))]
class PlayerController2D : PlayableCharacter
{
    [Header("Config")]
    [SerializeField] private CharacterStats stats;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform respawnPoint;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpSfx;
    [SerializeField] private AudioClip hurtSfx;

    public bool IsActive { get; private set; }

    private Rigidbody2D rb;
    private Collider2D col;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    private float moveInput;
    private bool jumpHeld;
    private bool jumpPressed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!IsActive) return;
        var kb = Keyboard.current;
        if (kb == null) return;

        moveInput = 0f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) moveInput -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) moveInput += 1f;

        jumpHeld = kb.spaceKey.isPressed;
        if (kb.spaceKey.wasPressedThisFrame) jumpPressed = true;

        if (moveInput != 0f && spriteRenderer != null)
            spriteRenderer.flipX = moveInput < 0f;
    }

    private void FixedUpdate()
    {
        if (!IsActive || rb.bodyType != RigidbodyType2D.Dynamic)
        {
            jumpPressed = false;
            return;
        }

        rb.linearVelocity = new Vector2(moveInput * stats.moveSpeed, rb.linearVelocity.y);

        if (stats.canFly)
        {
            if (jumpHeld)
                rb.linearVelocity += Vector2.up * (stats.flyAcceleration * Time.fixedDeltaTime);
            float y = Mathf.Clamp(rb.linearVelocity.y, stats.maxFallSpeed, stats.jumpForce);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, y);
        }
        else if (jumpPressed && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.jumpForce);
            PlaySfx(jumpSfx);
        }
        jumpPressed = false;
    }

    private bool IsGrounded()
    {
        Vector2 feet = col != null
            ? new Vector2(col.bounds.center.x, col.bounds.min.y)
            : (Vector2)transform.position;
        return Physics2D.OverlapCircle(feet, 0.15f, groundMask) != null;
    }

    /// Called by CharacterSwitcher. Inactive characters are kinematic and ignore input.
    public void SetControlled(bool active)
    {
        IsActive = active;
        moveInput = 0f;
        jumpHeld = false;
        jumpPressed = false;
        rb.bodyType = active ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        if (active && stats != null) rb.gravityScale = stats.gravityScale;
    }

    /// Respawn at the spawn point (called by base ApplyDamage when HP hits 0).
    public override void Die()
    {
        PlaySfx(hurtSfx);
        if (respawnPoint != null) transform.position = respawnPoint.position;
        rb.linearVelocity = Vector2.zero;
        currentHp = maxHp; // ready for the next mistake
    }

    private void PlaySfx(AudioClip clip)
    {
        if (audioSource != null && clip != null) audioSource.PlayOneShot(clip);
    }
}
