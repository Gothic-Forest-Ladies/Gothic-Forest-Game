using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// Bottom-of-screen hint text: controls intro at level start,
/// unlock messages, and a SPACE-ability reminder the first time each character is selected.
public class AbilityPromptUI : MonoBehaviour
{
    [SerializeField] private CharacterSwitcher switcher; // assigned in the inspector through Player
    [SerializeField] private Text promptText;            // legacy UI Text (project has no TMP)
    [SerializeField] private float promptDuration = 4f;
    [SerializeField] private string introMessage = "1 / 2 / 3 — switch animals";

    private readonly bool[] abilityPromptShown = new bool[3];
    private Coroutine hideRoutine;

    void Start()
    {
        if (switcher == null)
            switcher = FindAnyObjectByType<CharacterSwitcher>();

        if (switcher != null)
        {
            switcher.OnCharacterUnlocked += HandleUnlocked;
            switcher.OnCharacterChanged += HandleChanged;
        }

        ShowPrompt(introMessage);
    }

    void OnDestroy()
    {
        if (switcher != null)
        {
            switcher.OnCharacterUnlocked -= HandleUnlocked;
            switcher.OnCharacterChanged -= HandleChanged;
        }
    }

    private void HandleUnlocked(int index)
    {
        switch (index)
        {
            case 1:
                ShowPrompt("Owl unlocked! Press 2 to switch — SPACE to fly");
                break;
            case 2:
                ShowPrompt("Hedgehog unlocked! Press 3 to switch — SPACE to curl up");
                break;
        }
    }

    private void HandleChanged(int index)
    {
        if (index < 0 || index >= abilityPromptShown.Length || abilityPromptShown[index])
            return;

        abilityPromptShown[index] = true;

        switch (index)
        {
            case 0:
                ShowPrompt("Bear: hold SPACE next to a boulder to push it");
                break;
            case 1:
                ShowPrompt("Owl: press SPACE to fly");
                break;
            case 2:
                ShowPrompt("Hedgehog: press SPACE to curl into a ball");
                break;
        }
    }

    private void ShowPrompt(string message)
    {
        if (promptText == null)
            return;

        promptText.text = message;
        promptText.gameObject.SetActive(true);

        if (hideRoutine != null)
            StopCoroutine(hideRoutine);
        hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(promptDuration);
        promptText.gameObject.SetActive(false);
        hideRoutine = null;
    }
}
