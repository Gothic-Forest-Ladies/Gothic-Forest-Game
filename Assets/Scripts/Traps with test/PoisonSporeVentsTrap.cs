using UnityEngine;

public class PoisonSporeVentsTrap : AbstractTrap
{
    public GameObject player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
     public override void ApplyDamageToPlayer(IDamagable damagable)
        {
            Debug.Log($"[PoisonTrap] Applying damage to {damagable}");

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
            Debug.Log($"[PoisonTrap] Time: {Time.time:F3}");

            Debug.Log($"[PoisonTrap] Character Collider: {col.collider.name}, Type: {col.collider.GetType().Name}");

            Debug.Log($"[PoisonTrap] Trap Collider: {col.otherCollider.name}, Type: {col.otherCollider.GetType().Name}");

            Debug.Log($"[PoisonTrap] Contact Count: {col.contactCount}");

            Debug.Log($"[PoisonTrap] Collision with: {col.gameObject.name}, " + $"Tag: {col.gameObject.tag}, " + 
            $"IDamagable: {col.gameObject.GetComponent<IDamagable>() != null}");

           if (col.transform.tag == "Hedgehog") {
                Debug.Log("The trap has been touched by the Hedgehog!");
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
