using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// Put on an exit object with a trigger collider.
/// With nextSceneName set it loads that scene; empty = final exit:
/// shows the win text, freezes the game, and quits after a short delay.
public class LevelExit : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private GameObject winTextObject;
    [SerializeField] private float quitDelay = 4f; // seconds the win message stays before the game exits

    private bool triggered;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered)
            return;

        if (other.GetComponent<PlayableCharacter>() == null)
            return;

        triggered = true;

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            if (winTextObject != null)
                winTextObject.SetActive(true);
            Time.timeScale = 0f;
            StartCoroutine(QuitAfterDelay());
        }
    }

    // realtime wait: Time.timeScale is 0 while the win message is up
    private IEnumerator QuitAfterDelay()
    {
        yield return new WaitForSecondsRealtime(quitDelay);
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    void OnDestroy()
    {
        // safety: never leave the game frozen across a scene change
        if (triggered)
            Time.timeScale = 1f;
    }
}
