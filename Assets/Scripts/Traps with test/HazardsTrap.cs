using UnityEngine;

public class HazardsTrap : AbstractTrap
{   
    // Thorn pit / chasm 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     int damage = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
        public override void ApplyDamage(IDamagable damagable)
        {
            damagable.ApplyDamage(damage);
            Debug.Log("There was 100 damage done!");
            //Die();
        }

        //public override void Die();


        void OnCollisionEnter2D(Collision2D col)
        {
            //col.GetComponent<IDamagable>()?.ApplyDamage(damage);
            Debug.Log("The Trap has been touched! The player is dead ):");
        }

    /*
        public void layerMask(IDamagable whatIDamage)
        {
            //??
        }
    */
}
