using UnityEngine;

public class DitchTrap : AbstractTrap
{
    public int damage = 100;
    public GameObject player;



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
            player.GetComponent<PlayerBehaviour>().ApplyDamage(damage);

        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.transform.tag == "Boulder") {
                Debug.Log("The trap has been touched by the boulder!");
            }
            else {
                Debug.Log("The trap has been touched! Any Character will get hurt!");
                if (col.gameObject.GetComponent<IDamagable>() == null) return;
                ApplyDamageToPlayer(col.gameObject.GetComponent<IDamagable>());
            }
            

            // insert player knockback effect logic

            // insert player health reduction upon collision 
            // (decrease one 1 heart with contact with any character since only the boulder needs to touch it)


        }


    /*
        public void layerMask(IDamagable whatIDamage)
        {
            //??
        }
    */


        }
