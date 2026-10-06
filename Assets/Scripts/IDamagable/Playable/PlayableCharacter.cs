using UnityEngine;

public abstract class PlayableCharacter : MonoBehaviour, IDamagable
{
    
    // PlayableCharacter Variables / Ola 24.9.26
    [SerializeField] public float speed = 5f;
    [SerializeField] public float jumpForce = 8f;
    [SerializeField] public int maxHp;
    [SerializeField] public int currentHp; //v 
    
    // SerializeField is used to avoid other scripts from accessing that variable, that we want it to be visible in the inspector (usually private) - Kim.
    
    public virtual void ApplyDamage(int damage) //for now we dont know, maybe take 1 hp
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
    }
    // "die()" will probably move to the player itself, this one is just an abstract class for playable characters
    public virtual void Die()
    {
        // Place holder - decide what happens on death (respawn, game over, animation)
    }


    public virtual void SpecialAbility()
    {
        // Implementation for special ability
    }
    
    //  Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //set hp and maybe speed to default state?
        currentHp = maxHp;
    }

    public virtual void Movement(Rigidbody2D rb, float horizontalInput)
    {
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);
        
        if (horizontalInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
        public virtual void Jump(Rigidbody2D rb)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

}
