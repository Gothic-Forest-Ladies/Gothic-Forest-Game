using UnityEngine;

/// Data-driven per-character movement stats.
/// Create via: Assets > Create > GothicForest > Character Stats
[CreateAssetMenu(fileName = "CharacterStats", menuName = "GothicForest/Character Stats")]
public class CharacterStats : ScriptableObject
{
    public float moveSpeed = 6f;
    public float jumpForce = 11f;
    public float gravityScale = 3f;

    // FLIGHT TUNING: to change how the Bird flies, edit the values on
    // Assets/Data/BirdStats.asset in the Inspector. The flight code itself
    // is the marked FLIGHT MOVEMENT block in PlayerController2D.FixedUpdate.
    [Header("Flight (only used when canFly) - tune BirdStats.asset")]
    public bool canFly = false;
    [Tooltip("Upward push per second while holding Space. Higher = snappier rise.")]
    public float flyAcceleration = 30f;
    [Tooltip("Fall speed cap while flying. Small negative (-2) = slow glide, big negative = drops like a rock.")]
    public float maxFallSpeed = -20f;
}
