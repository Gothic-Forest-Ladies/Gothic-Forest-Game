using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    [SerializeField] private List<PlayableCharacter> characters;
    [SerializeField] private bool unlockAllAtStart = false; // Level 2 starts with everything unlocked
    private int currentIndex = 0; // essentially 0-> Bear, 1-> Owl, 2-> Mouse (Keybinds will be 1-Bear, 2-Owl, 3-Mouse)

    public PlayableCharacter ActiveCharacter => characters[currentIndex]; // It lets other scripts ask which character is selected
    public int CurrentIndex => currentIndex;

    // UI hooks: fired when the active character or an unlock changes
    public event Action<int> OnCharacterChanged;
    public event Action<int> OnCharacterUnlocked;

    private PlayableCharacter oldCharacter;
    private PlayableCharacter newCharacter;

    // Characters locked in the beginning of the game, will be unlocked later on in the game (19.10.26, Ola)
    public bool birdUnlocked = false;
    public bool mouseUnlocked = false;

    void Awake()
    {
        // death reloads the scene, so unlocks are restored from the static GameProgress
        birdUnlocked = GameProgress.BirdUnlocked || unlockAllAtStart;
        mouseUnlocked = GameProgress.MouseUnlocked || unlockAllAtStart;
    }

    public void Unlock(int index)
    {
        if (index == 1 && !birdUnlocked)
        {
            birdUnlocked = true;
            GameProgress.BirdUnlocked = true;
            OnCharacterUnlocked?.Invoke(1);
        }
        else if (index == 2 && !mouseUnlocked)
        {
            mouseUnlocked = true;
            GameProgress.MouseUnlocked = true;
            OnCharacterUnlocked?.Invoke(2);
        }
    }

    // used by RespawnManager: bypasses the unlock/CanSwitchAway checks and teleports
    public void ForceCharacter(int index, Vector3 position)
    {
        if (index < 0 || index >= characters.Count)
            return;

        ActiveCharacter.gameObject.SetActive(false);
        currentIndex = index;
        ActiveCharacter.transform.position = position;
        ActiveCharacter.gameObject.SetActive(true);
        OnCharacterChanged?.Invoke(currentIndex);
    }

    public void SelectCharacter(int index)
    {
        Debug.Log($"Received index: {index}");
        // index = requested character, currentIndex = currently selected character

        if (index < 0 || index >= characters.Count)
        {
            Debug.Log("Invalid character index! only 1, 2 or 3");
            return;
        }
        // Check if the character is unlocked before switching
        if (index == 1 && !birdUnlocked)
        {
            Debug.Log("The bird is still locked!");
            return;
        }
        // Check if the character is unlocked before switching
        if (index == 2 && !mouseUnlocked)
        {
            Debug.Log("The mouse is still locked!");
            return;
        }

        if (index == currentIndex)
        {
            Debug.Log("The character selected is already present!");
            return;
        }
        if (!ActiveCharacter.CanSwitchAway) // Added to prevent switching away from a character that is in the middle of an action (like crawling in a tight space) (17.10.26, Ola)
        {
            Debug.Log("Can't switch right now!");
            return;
        }


        oldCharacter = ActiveCharacter;
        oldCharacter.gameObject.SetActive(false);
        currentIndex = index;
        newCharacter = ActiveCharacter;
        newCharacter.gameObject.transform.position = oldCharacter.gameObject.transform.position;
        // making sure the new character appears from the old's coordinates
        newCharacter.gameObject.SetActive(true);

        OnCharacterChanged?.Invoke(currentIndex);
        }

    }

