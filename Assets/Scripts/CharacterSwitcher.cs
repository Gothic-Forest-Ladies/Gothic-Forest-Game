using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// Switches control between playable characters with keys 1..N.
/// The incoming character appears where the outgoing one stood
/// (shapeshift-style: only the active character is visible).
/// Add a third character later by just adding it to the list.
class CharacterSwitcher : MonoBehaviour
{
    [SerializeField] private List<PlayerController2D> characters = new List<PlayerController2D>();
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip switchSfx;

    private int activeIndex;

    public PlayerController2D Active =>
        characters.Count > 0 ? characters[Mathf.Clamp(activeIndex, 0, characters.Count - 1)] : null;

    private void Start()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].SetControlled(i == activeIndex);
            characters[i].gameObject.SetActive(i == activeIndex);
        }
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;
        for (int i = 0; i < characters.Count && i < 9; i++)
        {
            if (kb[(Key)((int)Key.Digit1 + i)].wasPressedThisFrame)
                Switch(i);
        }
    }

    public void Switch(int index)
    {
        if (index == activeIndex || index < 0 || index >= characters.Count) return;

        var from = characters[activeIndex];
        var to = characters[index];

        to.transform.position = from.transform.position;
        from.SetControlled(false);
        from.gameObject.SetActive(false);
        to.gameObject.SetActive(true);
        to.SetControlled(true);

        activeIndex = index;
        if (audioSource != null && switchSfx != null) audioSource.PlayOneShot(switchSfx);
    }
}
