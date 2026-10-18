using UnityEngine;
using UnityEngine.InputSystem;

public class Bear : PlayableCharacter
{
    [SerializeField] public float acceleration = 7f;   // units/sec^2 while speeding up - low value = heavy bear
    [SerializeField] public float deceleration = 10f;  // units/sec^2 while stopping
    
    [Header("Boulder Pushing")]
    [SerializeField] private float pushPower = 1f;
    [SerializeField] private Rigidbody2D rb;


    private void Awake()
    {
        if (rb == null) // make sure we have rb
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }
    public override void Movement(Rigidbody2D body, float horizontalInput, float verticalInput)
        {
            // Preserve the Bear's heavy acceleration and deceleration.
            float targetVx = horizontalInput * moveSpeed;

            float rate = Mathf.Abs(targetVx) > 0.01f ? acceleration : deceleration;

            float vx = Mathf.MoveTowards(body.linearVelocity.x, targetVx, rate * Time.fixedDeltaTime);

            body.linearVelocity = new Vector2(vx, body.linearVelocity.y);
        }


    // Special ability (passive)
     private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Boulder")) // if the object is not a boulder
        {
            return;
        }

        // Only push while the Bear is moving into the boulder.
        float inputX = horizontalMovement;

        if (Mathf.Abs(inputX) < 0.1f)
        {
            return;
        }

        Rigidbody2D boulder = collision.rigidbody;

        if (boulder == null) // if boulder doesnt have a rigidbody2D:
        {
            return;
        }

        // Only push if the input points toward the boulder.
        float direction = Mathf.Sign(inputX);
        float bearToBoulder = collision.transform.position.x - transform.position.x;

        if (direction * bearToBoulder <= 0f)
        {
            return;
        }

        boulder.AddForce(Vector2.right * direction * pushPower, ForceMode2D.Force);
    }
   
   
}
