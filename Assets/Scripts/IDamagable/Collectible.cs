using UnityEngine;

public class Collectible : MonoBehaviour
{
    public bool unlocksBird;
    public bool unlocksMouse;

    public CharacterSwitcher switcher;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayableCharacter>() == null)
        {
            return;   // not a character, do nothing
        }

        // unlock whichever character this collectible is for
        if (unlocksBird)
        {
            switcher.birdUnlocked = true;
        }

        if (unlocksMouse)
        {
            switcher.mouseUnlocked = true;
        }

        Destroy(gameObject);
    }
}
