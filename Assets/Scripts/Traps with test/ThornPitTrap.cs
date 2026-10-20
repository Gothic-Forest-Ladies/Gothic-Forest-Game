using UnityEngine;

public class ThornPitTrap : AbstractTrap
{   
    // The hazards trap will symbolize the thorn pit trap (for bird).
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int damage;
    public bool destroyableByBoulder; // true on the small spawnable thorns: a pushed boulder crushes them

        public override void ApplyDamageToPlayer(IDamagable damagable)
        {
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

            Debug.Log("The trap has been touched! Any Character will get hurt!");
            IDamagable damagable = col.gameObject.GetComponent<IDamagable>();

            if (damagable == null)
                return;            

            ApplyDamageToPlayer(damagable);
            
            // Get the playable character that collided with the trap.
            PlayableCharacter character = col.gameObject.GetComponent<PlayableCharacter>();

            if (character == null)
                return;

            // Push the character away from the trap: mostly sideways, barely up (the old 1.5 up felt like a bounce)
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
