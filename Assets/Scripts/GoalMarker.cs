using UnityEngine;

/// End-of-level marker. Logs completion and shows optional UI text.
class GoalMarker : MonoBehaviour
{
    [SerializeField] private GameObject completeUI;

    private bool done;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (done || other.GetComponent<PlayerController2D>() == null) return;
        done = true;
        Debug.Log("Demo complete!");
        if (completeUI != null) completeUI.SetActive(true);
    }
}
