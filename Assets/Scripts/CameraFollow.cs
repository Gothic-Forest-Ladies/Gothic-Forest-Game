using UnityEngine;

/// Smoothly follows the currently active character. No Cinemachine needed.
class CameraFollow : MonoBehaviour
{
    [SerializeField] private CharacterSwitcher switcher;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1.5f, -10f);
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 velocity;

    private void LateUpdate()
    {
        if (switcher == null || switcher.Active == null) return;
        Vector3 target = switcher.Active.transform.position + offset;
        target.z = offset.z;
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
    }
}
