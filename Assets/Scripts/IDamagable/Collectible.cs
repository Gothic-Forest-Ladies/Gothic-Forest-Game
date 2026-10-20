using UnityEngine;

public class Collectible : MonoBehaviour
{
    public bool unlocksBird;
    public bool unlocksMouse;

    public CharacterSwitcher switcher;

    void Start()
    {
        // after a death reload the unlock is already saved, so the pickup shouldn't reappear
        if ((unlocksBird && GameProgress.BirdUnlocked) ||
            (unlocksMouse && GameProgress.MouseUnlocked))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayableCharacter>() == null)
        {
            return;   // not a character, do nothing
        }

        // unlock whichever character this collectible is for
        if (unlocksBird)
        {
            switcher.Unlock(1);
        }

        if (unlocksMouse)
        {
            switcher.Unlock(2);
        }

        Destroy(gameObject);
    }
}
