using UnityEngine;

public class ThornPitTrap : AbstractTrap
{   
    // The hazards trap will symbolize the thorn pit trap (for bird).
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     int damage = 33;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
        public override void ApplyDamage(IDamagable damagable)
        {
            damagable.ApplyDamage(damage);
            Debug.Log("There was damage done! A player heart is lost!");
            //Die();
        }

        public override void Die()
        {
            // placeholder until we figure out how the trap "dies"

        }


        void OnCollisionEnter2D(Collision2D col)
        {
            Debug.Log("The trap has been touched!");
            //col.GetComponent<IDamagable>()?.ApplyDamage(damage);
            //GameObject gObj = col.gameObject;
            //gObj.GetComponent<IDamagable>()?.ApplyDamage(damage);
            ApplyDamage(damage);

            // insert player knockback effect logic


        }


    /*
        public void layerMask(IDamagable whatIDamage)
        {
            //??
        }
    */
}
