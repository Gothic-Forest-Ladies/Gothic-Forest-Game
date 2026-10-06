using UnityEngine;

public class DitchTrap : AbstractTrap
{
    int damage = 33;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
        public override void ApplyDamage(IDamagable damagable)
        {
            damagable.ApplyDamage(damage);
            Debug.Log("There was damage done! A player heart is lost!");
        }

        public override void Die()
        {
            // placeholder until we figure out how the trap "dies"
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            
            Debug.Log("The trap has been touched!");
            //col.GetComponent<IDamagable>()?.ApplyDamage(damage); // the problem is, there is no way to activate get component on a Collision2D variable (and not a GameObject)
            //GameObject gameObject = col.gameObject;
            //gameObject.GetComponent<IDamagable>()?.ApplyDamage(damage);
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
