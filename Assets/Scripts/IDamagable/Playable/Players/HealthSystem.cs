
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHp = 3; // 3 heartts
    [SerializeField] private int currentHp;
    [SerializeField] protected float invulnerabilityDuration = 2.0f;
    private CharacterSwitcher characterSwitcher;   //CharacterSwitcher activeCharacter;
    PlayableCharacter activeCharacter;
    private bool hasDiedOnce = false; // trigger death only once
    private bool isInvulnerable = false; // damage cooldown from traps (invulnerable state = iframe)

     // Other scripts can read these values, but cannot set them directly.
    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;
    private void Awake()
    {
        currentHp = maxHp; // initialize player

        // CharacterSwitcher is on the parent Player object.
        characterSwitcher = GetComponent<CharacterSwitcher>();

        if (characterSwitcher == null)
        {
            Debug.LogError("CharacterSwitcher not found on Player!");
        }
    }

    public void TakeDamage(int amount)
    {

     // Ignore further damage after death, or invalid damage amounts.
        if (hasDiedOnce || amount <= 0)
        {
            return;
        }

        if (!isInvulnerable) 
        { 
            Debug.Log("[HealthSystem] Invulnerability started!");
            // Reduce shared health, without allowing negative HP.
            currentHp = Mathf.Max(0, currentHp - amount);
            Debug.Log($"Shared health: {currentHp}/{maxHp}");

            // Check health after applying damage.
            if (currentHp == 0)
            {
                hasDiedOnce = true;
                activeCharacter = characterSwitcher.ActiveCharacter;
                activeCharacter.Die();
                return;
            }

            // damage cooldown state
            isInvulnerable = true;
            StartCoroutine(nameof(EndInvulnerabilityCD));
        }


    }

    private System.Collections.IEnumerator EndInvulnerabilityCD()
    {
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;

        Debug.Log("[HealthSystem] Invulnerability ended!");
    }

   

}
