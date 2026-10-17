using UnityEngine;

public class Mouse : PlayableCharacter
{
    [SerializeField] public Rigidbody2D rb;

    [Header("Roll Ability")]
    [SerializeField] private float rollScale = 0.5f; // makes it smaller when rolling
    [SerializeField] private float rollSpeedMultiplier = 1.8f;// makes it faster when rolling
    [SerializeField] private LayerMask solidLayers;// ground, walls, platforms

    private bool isRolling = false;
    private Vector3 normalScale;
    private float normalSpeed;
    private Collider2D col;
    private SpriteRenderer sprite;

    void Awake()
    {
        normalScale = transform.localScale;// remember the original size
        normalSpeed = moveSpeed;// remember the original speed
        col = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public override void Movement(Rigidbody2D rb, float horizontalInput, float verticalInput)
    {
        if (horizontalInput > 0.1f)
        {
            sprite.flipX = false;
        }
        else if (horizontalInput < -0.1f)
        {
            sprite.flipX = true;
        }

        base.Movement(rb, horizontalInput, verticalInput);
    }
    // The switcher asks this before changing character
    public override bool CanSwitchAway => !isRolling;

    public override void SpecialAbility()
    {
        if (!isRolling)
        {
            StartRolling();
        }
        else if (HasRoomToUncurl())
        {
            StopRolling();
        }
        else
        {
            Debug.Log("No room to uncurl!");
        }
    }

    private void StartRolling()
    {
        isRolling = true;
        transform.localScale = normalScale * rollScale;
        moveSpeed = normalSpeed * rollSpeedMultiplier;
    }

    private void StopRolling()
    {
        isRolling = false;
        transform.localScale = normalScale;
        moveSpeed = normalSpeed;
    }

    // Is there empty space for the normal-sized body?
    private bool HasRoomToUncurl()
    {
        // size the collider would have after un-rolling
        Vector2 normalSize = col.bounds.size / rollScale;

        // box centered so the feet stay where they are
        Vector2 center = new Vector2(col.bounds.center.x,
                                     col.bounds.min.y + normalSize.y / 2f);

        // slightly smaller box, so touching the floor doesn't count as blocked
        bool blocked = Physics2D.OverlapBox(center, normalSize * 0.9f, 0f, solidLayers);
        return !blocked;
    }
    
    public override void TakeDamageFromTrap(IDamagable damagable) // apply dmg to trap (?)
    {
        // maybe when passed through successfully, it can take damage from the character player and self destruct or die?
        damagable.ApplyDamage(damage);
        Debug.Log("There was damage done! A player heart is lost!");
    } 

     public override void ApplyDamage(int damage)
    {
         // apply dmg to trap (?)/ take dmg from trap
          currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }

    }

}
