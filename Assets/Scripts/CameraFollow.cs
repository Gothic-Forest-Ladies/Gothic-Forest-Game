using UnityEngine;

/// Smoothly follows the player horizontally, clamped to the level bounds.
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.2f;
    public float fixedY = -0.5f;
    public float minX = -5.4f;
    public float maxX = 19.7f;

    Vector3 velocity;

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;
        float x = Mathf.Clamp(target.position.x, minX, maxX);
        Vector3 goal = new Vector3(x, fixedY, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, goal, ref velocity, smoothTime);
    }
}
