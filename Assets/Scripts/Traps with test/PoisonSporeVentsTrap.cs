using UnityEngine;

public class PoisonSporeVentsTrap : AbstractTrap
{
    public int damage;
    public bool destroyableByBoulder; // true only on the standalone spawnable vent, never on the cave floor
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
            Debug.Log($"Trap {name} was crushed by the boulder!");
            Destroy(gameObject);
        }

        public override void ApplyDamage(int damage)
        {   
            // insert apply damage to player health logic here
        }

         void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Boulder"))
            {
                if (destroyableByBoulder)
                    Die();
                return;
            }

           // Hedgehog is immune to poison spores, but only while curled up.
            Hedgehog hedgehog = col.gameObject.GetComponent<Hedgehog>();
            if (hedgehog != null && hedgehog.IsRolling)
            {
                Debug.Log("The curled-up Hedgehog rolls safely over the spores!");
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

            // mostly sideways, barely up (the old 1.5 up felt like a bounce)
            float horizontalDirection = character.transform.position.x < transform.position.x ? -1f : 1f;
            Vector2 direction = new Vector2(horizontalDirection, 0.3f);

            character.Knockback(direction);

        }


    /*
        public void layerMask(IDamagable whatIDamage)
        {
            //??
        }
    */
}
