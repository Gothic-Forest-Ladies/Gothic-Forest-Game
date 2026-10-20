using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public abstract class PlayableCharacter : MonoBehaviour, IDamagable
{
    
    // PlayableCharacter Variables / Ola 24.9.26
    // Changes and additions / Kim 10.10.26
    // Added some changes for new characters / Ola 17.10.26
    // Updated changes for characters + HealthSystem / Kim 

    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] private int currentHp;
    [SerializeField] private int maxHp;
    [SerializeField] public float horizontalMovement;
    [SerializeField] public int damage;
    public virtual bool CanSwitchAway => true;
    HealthSystem healthSystem;
    // knockback logic
    [SerializeField] protected float knockbackForce = 6f;
    [SerializeField] protected float knockbackDuration = 0.2f; // duration in which movement is canceled
    public bool IsKnockedBack { get; protected set; } // a variable that CharacterMovement can access to determine if kb is active

    
    //  Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //set hp and maybe speed to default state?
        currentHp = maxHp;
    }

    protected virtual void OnEnable()
    {
        // the knockback coroutine dies with the object; never come back stuck mid-knockback
        IsKnockedBack = false;
    }

    void Update()
    {
        //rb.linearVelocity = new Vector2(horizontalMovement * speed, rb.linearVelocity.y);

    }

    // IDamagable Interface Implementation
    public virtual void ApplyDamage(int damage) //take damage (?)
    {
        healthSystem = GetComponentInParent<HealthSystem>();

        if (healthSystem == null)
        {
            Debug.LogError("HealthSystem not found on Player!");
            return;
        }

        healthSystem.TakeDamage(1);
    }

    // "die()" will probably move to the player itself, this one is just an abstract class for playable characters
    public virtual void Die()
    {
        Debug.Log("The player has died ): Game over");

        // soft respawn (camera pan + fog) when a RespawnManager is on the Player
        RespawnManager respawn = GetComponentInParent<RespawnManager>();
        if (respawn != null)
        {
            respawn.BeginRespawn();
            return;
        }

        // fallback: old behaviour, reload the scene
        Debug.Log("The player has died! Reloading scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public virtual void TakeDamageFromTrap(IDamagable damagable)
    {
        damagable.ApplyDamage(damage);
        Debug.Log("Damage was taken from trap! one heart lost! ");
    }


    // will also move over to the actual character itself
    public virtual void SpecialAbility()
    {
        // Implementation for special ability (each animal is different)
    }

    // called when the ability key is released (used by the Bear's hold-to-push)
    public virtual void SpecialAbilityReleased()
    {
    }
    

    /*
    // Jump was canceled, will probably delete it, but leaving it here for now in case we want to add it back in later
    public virtual void Jump(Rigidbody2D rb)
    {
          rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);    
    }
    */

    // PlayableCharacter.Movement() moves the character.
    // BearAnimationDriver turns the sprite left or right.
    // rb Uses the horizontal input to update the active character's Rigidbody2D velocity.
    public virtual void Movement(Rigidbody2D rb, float horizontalInput, float verticalMovement)
    {
        //Debug.Log("Movement called");
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        
    }

    // knockback logic for Player
    public virtual void Knockback(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>(); // make sure to get rb for velocity

        if (rb == null)
            return;

        // The coroutine gives physics a short period to move the character before normal player input takes control again.
        StopCoroutine(nameof(EndKnockback));

        IsKnockedBack = true;
            Debug.Log(
        $"[Knockback] Character: {gameObject.name}, " + $"Direction: {direction}, " + $"Force: {knockbackForce}, " + $"Velocity before: {rb.linearVelocity}");

        rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
        
        Debug.Log($"[Knockback] Impulse requested for {gameObject.name}");

        StartCoroutine(EndKnockback()); 
    }

    private System.Collections.IEnumerator EndKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration);

        IsKnockedBack = false;
    }
    
    

}
