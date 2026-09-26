using UnityEngine;

/// Put on a trigger collider (e.g. the Hazards tilemap).
/// Damages anything IDamagable that touches it.
class Hazard : MonoBehaviour
{
    [SerializeField] private int damage = 999;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<IDamagable>()?.ApplyDamage(damage);
    }
}
