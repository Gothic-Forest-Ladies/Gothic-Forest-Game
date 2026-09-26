using UnityEngine;

/// Data-driven per-character movement stats.
/// Create via: Assets > Create > GothicForest > Character Stats
[CreateAssetMenu(fileName = "CharacterStats", menuName = "GothicForest/Character Stats")]
public class CharacterStats : ScriptableObject
{
    public float moveSpeed = 6f;
    public float jumpForce = 11f;
    public float gravityScale = 3f;

    [Header("Flying (only used when canFly)")]
    public bool canFly = false;
    public float flyAcceleration = 30f;
    public float maxFallSpeed = -20f;
}
