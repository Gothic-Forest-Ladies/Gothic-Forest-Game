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

           // Hedgehog is immune to poison spores.
            if (col.gameObject.CompareTag("Hedgehog"))
            {
                Debug.Log("The trap has been touched by the Hedgehog!");
                return;
            }
           
            IDamagable damagable = col.gameObject.GetComponent<IDamagable>();

            if (damagable == null)
                return;

            Debug.Log("The trap has been touched! Wrong Character! (Bird/Bear)");
            ApplyDamageToPlayer(damagable);

            // Knockback applies only to Bird/Bear playable characters.
            PlayableCharacter character = col.gameObject.GetComponent<PlayableCharacter>();

            if (character == null) // make sure we got the character
                return;

            float horizontalDirection = character.transform.position.x < transform.position.x ? -1f : 1f;
            Vector2 direction = new Vector2(horizontalDirection, 1.5f);

            character.Knockback(direction);

        }


    /*
        public void layerMask(IDamagable whatIDamage)
        {
            //??
        }
    */
}
