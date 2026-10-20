using UnityEngine;

public class DitchTrap : AbstractTrap
{
    public int damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
        public override void ApplyDamageToPlayer(IDamagable damagable)
        {
            damagable.ApplyDamage(damage);
            Debug.Log("There was damage done! A player heart is lost!");
        }

        public override void Die()
        {
            // placeholder until we figure out how the trap "dies"
        }

        public override void ApplyDamage(int damage)
        {
            // insert apply damage to player health logic here
            //player.GetComponent<Bear>().ApplyDamage(damage);

        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Boulder"))
            {
                Debug.Log("The trap has been touched by the boulder!");
                return;
            }

            Debug.Log("The trap has been touched! Any Character will get hurt!");

            IDamagable damagable = col.gameObject.GetComponent<IDamagable>();

            if (damagable == null)
                return;
            ApplyDamageToPlayer(damagable);

            // Apply knockback to all playable characters
            PlayableCharacter character = col.gameObject.GetComponent<PlayableCharacter>();

            if (character == null) // make sure we got the character
                return;

            // the ditch keeps a strong upward knockback on purpose: with no jump,
            // it is the only way out of the pit for a character that fell in
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
