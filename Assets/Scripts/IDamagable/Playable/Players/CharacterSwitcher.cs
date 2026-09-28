using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    [SerializeField] private List<PlayableCharacter> characters;
    private int currentIndex = 0;

    public PlayableCharacter ActiveCharacter => characters[currentIndex];
}
