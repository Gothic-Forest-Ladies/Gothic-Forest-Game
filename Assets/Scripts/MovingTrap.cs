using UnityEngine;

// Spike block that patrols back and forth between two points and hurts
// whatever it touches. With 'activateOnFlagCollected' on, it stays dormant
// (dimmed, not moving, no damage) until the first Flag is collected.
public class MovingTrap : MonoBehaviour
{
    [SerializeField] Vector2 pointA;
    [SerializeField] Vector2 pointB;
    [SerializeField] float speed = 2f;
    [SerializeField] int damage = 1;
    [SerializeField] bool activateOnFlagCollected = true;
    [SerializeField, Range(0f, 1f)] float dormantAlpha = 0.35f;

    bool isActive;
    float travel;    // distance moved along the A->B segment
    int direction = 1;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        Flag.FlagCollected += OnFlagCollected;
    }

    void OnDisable()
    {
        Flag.FlagCollected -= OnFlagCollected;
    }

    void Start()
    {
        isActive = !activateOnFlagCollected || Flag.AnyCollected;
        transform.position = pointA;
        ApplyVisualState();
    }

    void OnFlagCollected(Flag flag)
    {
        if (isActive)
            return;
        isActive = true;
        ApplyVisualState();
    }

    void ApplyVisualState()
    {
        if (spriteRenderer == null)
            return;
        Color color = spriteRenderer.color;
        color.a = isActive ? 1f : dormantAlpha;
        spriteRenderer.color = color;
    }

    void Update()
    {
        if (!isActive)
            return;

        float length = Vector2.Distance(pointA, pointB);
        if (length < 0.001f)
            return;

        travel += direction * speed * Time.deltaTime;
        if (travel >= length)
        {
            travel = length;
            direction = -1;
        }
        else if (travel <= 0f)
        {
            travel = 0f;
            direction = 1;
        }

        transform.position = Vector2.Lerp(pointA, pointB, travel / length);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive)
            return;
        other.GetComponentInParent<IDamagable>()?.ApplyDamage(damage);
    }

    // Draw the patrol path in the Scene view for hand-tuning.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(pointA, pointB);
        Gizmos.DrawWireSphere(pointA, 0.15f);
        Gizmos.DrawWireSphere(pointB, 0.15f);
    }
}
