using UnityEngine;

public class PoisonSporeVentsTrap : AbstractTrap
{
    public int damage = 100;
    public GameObject player;


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
            // playerScript = player.GetComponent<PlayerBehaviour>();
            player.GetComponent<PlayerBehaviour>().ApplyDamage(damage);
        }

         void OnCollisionEnter2D(Collision2D col)
        {
           if (col.transform.tag == "Mouse") {
                Debug.Log("The trap has been touched by the Mouse!");
            }
            else {
                Debug.Log("The trap has been touched! Wrong Character! (Bird/Bear)");
                if (col.gameObject.GetComponent<IDamagable>() == null) return;
                ApplyDamageToPlayer(col.gameObject.GetComponent<IDamagable>());
            }
            

            // insert player knockback effect logic

            // insert player health reduction upon collision 
            // (decrease one 1 heart with contact with the wrong character (bird, bear) since the mouse needs to go inside and cross it)
            

        }


    /*
        public void layerMask(IDamagable whatIDamage)
        {
            //??
        }
    */
}
