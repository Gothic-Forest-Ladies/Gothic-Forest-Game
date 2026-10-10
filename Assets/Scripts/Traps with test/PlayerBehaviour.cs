using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour, IDamagable
{
    public Rigidbody2D rb;
    public float moveSpeed = 3.2f;
    public float acceleration = 7f;   // units/sec^2 while speeding up - low value = heavy bear
    public float deceleration = 10f;  // units/sec^2 while stopping
    public int currentHp;
    public int maxHp = 100;
    public float horizontalMovement;
    [SerializeField] public float pushPower = 1.0f;

        // SerializeField is used to avoid other scripts from accessing that variable, that we want it to be visible in the inspector (usually private)


    // Start is called once before the first execution of Update after the MonoBehaviour is created
  

    void Start()
    {
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        // ramp toward the target speed instead of snapping to it, so the bear feels heavy
        float targetVx = horizontalMovement * moveSpeed;
        float rate = Mathf.Abs(targetVx) > 0.01f ? acceleration : deceleration;
        float vx = Mathf.MoveTowards(rb.linearVelocity.x, targetVx, rate * Time.deltaTime);
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;

    }

    public virtual void Die()
    {
        Debug.Log("The player has died ): Game over");
        // Place holder - decide what happens on death (respawn, game over, animation)
    }

    public virtual void ApplyDamage(int damage) //for now we dont know, maybe take 1 hp 
    { // take damage from trap
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    // insert push/pull function logic for boulder (bear)
    public void onControllerColliderHit(ControllerColliderHit hit)
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

}
