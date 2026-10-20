using System.Collections;
using UnityEngine;

/// A boulder only the Bear's SPACE push can move.
/// Kinematic at rest, so walking into it never moves it. While the Bear holds SPACE
/// next to it, PushTick slides it smoothly each physics step; when it is pushed past
/// an edge (no ground underneath) it turns dynamic and falls.
[RequireComponent(typeof(Rigidbody2D))]
public class PushableBoulder : MonoBehaviour
{
    [SerializeField] private float pushSpeed = 2.5f;
    [SerializeField] private LayerMask groundMask = 1 << 8;      // Ground ONLY — traps below must not count as floor
    [SerializeField] private LayerMask wallMask = (1 << 8) | 1;  // Ground + Default: walls and characters block the push
    [SerializeField] private bool shatterOnSettle = false;       // ledge boulder: crumble after the fall so it never blocks the path
                                                                 // (the ditch boulder keeps this off — it must stay as the bridge)

    private Rigidbody2D rb;
    private Collider2D col;
    private bool released; // went dynamic after being pushed over an edge

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    /// Called by the Bear every FixedUpdate while SPACE is held next to the boulder.
    /// Returns true while the boulder actually moves (drives the push animation).
    public bool PushTick(float direction)
    {
        if (released || Mathf.Abs(direction) < 0.01f)
            return false;

        float dir = Mathf.Sign(direction);

        // over an edge already? let go and fall
        RaycastHit2D ground = Physics2D.Raycast(col.bounds.center, Vector2.down, col.bounds.extents.y + 0.4f, groundMask);
        if (ground.collider == null)
        {
            Release(dir);
            return false;
        }

        // blocked ahead? probe only a thin strip past the leading edge, so neither the
        // boulder itself nor the bear pushing from behind can ever count as a wall
        Vector2 probeCenter = (Vector2)col.bounds.center + new Vector2(dir * (col.bounds.extents.x + 0.15f), 0f);
        Vector2 probeSize = new Vector2(0.2f, col.bounds.size.y * 0.5f);
        if (Physics2D.OverlapBox(probeCenter, probeSize, 0f, wallMask) != null)
            return false;

        rb.MovePosition(rb.position + new Vector2(dir * pushSpeed * Time.fixedDeltaTime, 0f));
        return true;
    }

    private void Release(float dir)
    {
        released = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.WakeUp(); // a body converted from kinematic can be asleep and would hang in the air
        rb.linearVelocity = new Vector2(dir * pushSpeed, 0f);

        if (shatterOnSettle)
            StartCoroutine(ShatterWhenSettled());
    }

    // after the fall, once the boulder stops moving it crumbles away,
    // so it can never end up blocking the way to the exit
    private IEnumerator ShatterWhenSettled()
    {
        yield return new WaitForSeconds(0.3f); // let it actually fall first

        float stillTime = 0f;
        while (stillTime < 0.4f)
        {
            yield return new WaitForFixedUpdate();
            stillTime = rb.linearVelocity.magnitude < 0.25f ? stillTime + Time.fixedDeltaTime : 0f;
        }

        Debug.Log("The boulder smashed the traps below and crumbled!");
        Destroy(gameObject);
    }
}
