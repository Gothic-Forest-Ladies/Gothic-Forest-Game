using UnityEngine;

/// Smoothly follows the player horizontally, clamped to the level bounds.
/// Looks ahead in the walk direction so the player sees where they are going.
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;
    public float lookAhead = 1.5f;       // how far ahead of the bear the camera aims
    public float lookAheadSpeed = 2f;    // how fast the look-ahead shifts when turning around
    public float fixedY = -1.3f;
    public float minX = -13.5f;
    public float maxX = 27.9f;

    Vector3 velocity;
    Rigidbody2D targetRb;
    float look;

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }
        if (target != null) targetRb = target.GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        if (target == null) return;
        float vx = targetRb != null ? targetRb.linearVelocity.x : 0f;
        // keep the last look direction while standing still so the camera doesn't drift back
        if (Mathf.Abs(vx) > 0.1f)
        {
            float desired = Mathf.Sign(vx) * lookAhead;
            look = Mathf.MoveTowards(look, desired, lookAheadSpeed * Time.deltaTime);
        }
        float x = Mathf.Clamp(target.position.x + look, minX, maxX);
        Vector3 goal = new Vector3(x, fixedY, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, goal, ref velocity, smoothTime);
    }
}
