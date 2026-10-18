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

    
    //  Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //set hp and maybe speed to default state?
        currentHp = maxHp;
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
        // insert scene reload logic.
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
        
        /*
        if (horizontalInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        //Debug.Log($"{horizontalInput}, {rb.linearVelocity}, {rb.bodyType}");
        */
    }
    
    

}
