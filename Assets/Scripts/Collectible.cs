using UnityEngine;

/// Simple pickup: plays a sound and disappears when a player touches it.
class Collectible : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSfx;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController2D>() == null) return;
        Debug.Log("Collected: " + name);
        if (pickupSfx != null) AudioSource.PlayClipAtPoint(pickupSfx, transform.position);
        Destroy(gameObject);
    }
}
