using UnityEngine;

public class SquareSpikesTrap : AbstractTrap
{
    int damage = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
        public override void ApplyDamage(IDamagable damagable)
        {
            damagable.ApplyDamage(damage);
            Debug.Log("There was 1 damage done!");
        }

        //public override void Die();

        void OnCollisionEnter2D(Collision2D col)
        {
            
            //col.GetComponent<IDamagable>()?.ApplyDamage(damage);
            Debug.Log("The Trap has been touched!");
        }

    /*
        public void layerMask(IDamagable whatIDamage)
        {
            //??
        }
    */

}
