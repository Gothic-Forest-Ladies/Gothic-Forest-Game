using UnityEngine;

public abstract class AbstractTrap : MonoBehaviour, IDamagable 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool dead;
    //1
    // child classes inheriting this will be forced to implement these methods
    public abstract void ApplyDamage(IDamagable damagable); 
    
    //2
    //public abstract void layerMask(IDamagable whatIDamage);
    //layerMask whatIDamage //optional implementation

    //3
    public void ApplyDamage(int damage)
    {
        Debug.Log($"There was {damage} damage done! ");
    }
    public void Die()
    {   
        dead = true;
    }



}
