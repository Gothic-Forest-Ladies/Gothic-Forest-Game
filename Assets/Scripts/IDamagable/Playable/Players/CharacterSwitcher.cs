using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    [SerializeField] private List<PlayableCharacter> characters;
    private int currentIndex = 0; // essentially 0-> Bear, 1-> Owl, 2-> Mouse (Keybinds will be 1-Bear, 2-Owl, 3-Mouse)

    public PlayableCharacter ActiveCharacter => characters[currentIndex]; // It lets other scripts ask which character is selected

    private PlayableCharacter oldCharacter;
    private PlayableCharacter newCharacter;

    void Start()
    {
            
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
        
        }

    }

