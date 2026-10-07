using UnityEngine;

public class ThornPitTrap : AbstractTrap
{   
    // The hazards trap will symbolize the thorn pit trap (for bird).
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     int damage = 33;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
        public override void ApplyDamageToPlayer(IDamagable damagable)
        {
            damagable.ApplyDamage(damage);
            Debug.Log("There was damage done! A player heart is lost!");
            //Die();
        }

        public override void Die()
        {
            // placeholder until we figure out how the trap "dies"

        }

        public override void ApplyDamage(int damage)
        {
            // insert apply damage to player health logic here
        }


        void OnCollisionEnter2D(Collision2D col)
        {
            Debug.Log("The trap has been touched! Any Character will get hurt!");
            if (col.gameObject.GetComponent<IDamagable>() == null) return;
            ApplyDamageToPlayer(col.gameObject.GetComponent<IDamagable>());
            

            // insert player knockback effect logic

            // insert player health reduction upon collision 
            // (decrease one 1 heart with contact with any character since the bird needs to fly over it)

        }


    /*
        public void layerMask(IDamagable whatIDamage)
        {
            //??
        }
    */
}
