using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayableCharacter : MonoBehaviour, IDamagable
{
    
    // PlayableCharacter Variables / Ola 24.9.26
    // Changes and additions / Kim 10.10.26

    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public int currentHp;
    [SerializeField] public int maxHp = 100;
    [SerializeField] public float horizontalMovement;
    [SerializeField] public float acceleration = 7f;   // units/sec^2 while speeding up - low value = heavy bear
    [SerializeField] public float deceleration = 10f;  // units/sec^2 while stopping
    [SerializeField] public float jumpForce = 8f;
    //public Rigidbody2D rb; 
    [SerializeField] public int damage = 100;

    
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
      //damage applied idk
    }

    // "die()" will probably move to the player itself, this one is just an abstract class for playable characters
    public virtual void Die()
    {
        // Place holder - decide what happens on death (respawn, game over, animation)
        Debug.Log("The player has died ): Game over");

    }

    public abstract void TakeDamageFromTrap(IDamagable damagable); // take dmg from trap (?)


    // will also move over to the actual character itself
    public virtual void SpecialAbility()
    {
        // Implementation for special ability
    }
    

    public virtual void Jump(Rigidbody2D rb)
    {
          rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);    
    }

    // PlayableCharacter.Movement() moves the character.
    // BearAnimationDriver turns the sprite left or right.
    // Uses the horizontal input to update the active character's Rigidbody2D velocity.
    public virtual void Movement(Rigidbody2D rb, float horizontalInput)
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
