using System;
using UnityEngine;

// Flag checkpoint the player collects by touching it.
// Raises a static event so other objects (like MovingTrap) can react.
[RequireComponent(typeof(Collider2D))]
public class Flag : MonoBehaviour
{
    public static event Action<Flag> FlagCollected;
    public static bool AnyCollected { get; private set; }

    [SerializeField] Sprite collectedSprite; // shown after the flag is taken

    public bool IsCollected { get; private set; }

    // Statics survive entering play mode with domain reload off - reset them here.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        AnyCollected = false;
        FlagCollected = null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsCollected)
            return;

        // Accept the player: anything damagable or any moving (rigidbody) collider.
        bool isCharacter = other.attachedRigidbody != null
                           || other.GetComponentInParent<IDamagable>() != null;
        if (!isCharacter)
            return;

        IsCollected = true;
        AnyCollected = true;

        if (collectedSprite != null)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                spriteRenderer.sprite = collectedSprite;
        }

        FlagCollected?.Invoke(this);
    }
}
