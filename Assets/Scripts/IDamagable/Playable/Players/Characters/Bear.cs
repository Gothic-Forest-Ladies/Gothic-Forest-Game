using UnityEngine;
using UnityEngine.InputSystem;

public class Bear : PlayableCharacter
{
    [SerializeField] public float pushPower = 1.0f; // only for bear
    [SerializeField] public Rigidbody2D rb;
    
    //[SerializeField] public float acceleration = 7f;   // units/sec^2 while speeding up - low value = heavy bear
    //[SerializeField] public float deceleration = 10f;  // units/sec^2 while stopping
    //public GameObject player;


    void Update()
    {
        // ramp toward the target speed instead of snapping to it, so the bear feels heavy
        float targetVx = horizontalMovement * moveSpeed;
        float rate = Mathf.Abs(targetVx) > 0.01f ? acceleration : deceleration;
        float vx = Mathf.MoveTowards(rb.linearVelocity.x, targetVx, rate * Time.deltaTime);
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
    }
 
    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void TakeDamageFromTrap(IDamagable damagable) // apply dmg to trap (?)
    {
        // maybe when passed through successfully, it can take damage from the character player and self destruct or die?
        damagable.ApplyDamage(damage);
        Debug.Log("There was damage done! A player heart is lost!");
    } 


    public override void ApplyDamage(int damage)
    {
         // apply dmg to trap (?)/ take dmg from trap
          currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }

    }

    // cannot reference the Movement function from PlayableCharacter abstract class so for now I wrote this


     // insert push/pull function logic for boulder (bear)
    public void onControllerColliderHit(ControllerColliderHit hit) // Special Ability
    {
        if (hit.transform.tag == "Boulder")
        {
            Rigidbody2D box = hit.collider.GetComponent<Rigidbody2D>();
            
            if (box != null)
            {
                Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, 0);
                box.linearVelocity = pushDir * pushPower;
            }
        }
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    // override public void Movement()
    // {

    // }
}
