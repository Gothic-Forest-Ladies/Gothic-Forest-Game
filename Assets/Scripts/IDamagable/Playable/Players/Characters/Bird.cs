using UnityEngine;

public class Bird : PlayableCharacter
{

    //[SerializeField] public float jumpForce = 8f;
    [SerializeField] public Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
/*
        public virtual void Jump(Rigidbody2D rb)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);    
    }
    */
}
