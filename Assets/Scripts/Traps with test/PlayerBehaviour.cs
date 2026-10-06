using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public int currentHp;
    public int maxHp = 100;
    public float horizontalMovement;
    [SerializeField] public float pushPower = 2.0f;

        // SerializeField is used to avoid other scripts from accessing that variable, that we want it to be visible in the inspector (usually private)


    // Start is called once before the first execution of Update after the MonoBehaviour is created
  

    void Start()
    {
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;

    }

    public virtual void Die()
    {
        // Place holder - decide what happens on death (respawn, game over, animation)
    }

    public virtual void ApplyDamage(int damage) //for now we dont know, maybe take 1 hp
    {
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
