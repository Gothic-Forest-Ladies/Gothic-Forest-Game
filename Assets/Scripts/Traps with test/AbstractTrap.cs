using UnityEngine;

public abstract class AbstractTrap : MonoBehaviour, IDamagable 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // child classes inheriting this will be forced to implement these methods
    public abstract void ApplyDamageToPlayer(IDamagable damagable); // apply dmg to player
    
    //2
    //public abstract void layerMask(IDamagable whatIDamage);
    //layerMask whatIDamage //optional implementation
    //whatIDamage - I decide who to damage.. which type of character.

    //3
    public virtual void ApplyDamage(int damage)
    {
        // Debug.Log($"There was {damage} damage done! ");
    }

     public virtual void Die()
    {
        // Place holder - decide what happens on death (?) 
    }

    //public abstract void Die();
 



}
