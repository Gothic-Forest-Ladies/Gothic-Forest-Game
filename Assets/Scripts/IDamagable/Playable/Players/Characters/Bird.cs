using UnityEngine;

public class Bird : PlayableCharacter
{

    public GameObject player;
    //[SerializeField] public float jumpForce = 8f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void TakeDamageFromTrap(IDamagable damagable) // take dmg from trap (?)
    {
        // maybe when passed through successfully, it can take damage from the character player and self destruct or die?
    } 

/*
        public virtual void Jump(Rigidbody2D rb)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);    
    }
    */
}
