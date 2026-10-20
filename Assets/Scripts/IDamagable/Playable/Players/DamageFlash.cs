using System.Collections;
using UnityEngine;

/// Sits on the Player next to HealthSystem. When HP drops, the active
/// character's sprite blinks brightly so the hit reads at a glance.
public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor = new Color(1f, 0.35f, 0.35f);
    [SerializeField] private int blinkCount = 3;
    [SerializeField] private float blinkInterval = 0.08f;

    private HealthSystem healthSystem;
    private CharacterSwitcher switcher;
    private int oldHp = -1;
    private SpriteRenderer flashingSprite;

    void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        switcher = GetComponent<CharacterSwitcher>();
    }

    void Start()
    {
        if (healthSystem != null)
            oldHp = healthSystem.CurrentHp;
    }

    void Update()
    {
        if (healthSystem == null || switcher == null)
            return;

        int hp = healthSystem.CurrentHp;
        if (hp < oldHp && switcher.ActiveCharacter != null)
        {
            var sr = switcher.ActiveCharacter.GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                StopAllCoroutines();
                if (flashingSprite != null)
                    flashingSprite.color = Color.white; // restore a sprite caught mid-flash
                flashingSprite = sr;
                StartCoroutine(Flash(sr));
            }
        }
        oldHp = hp;
    }

    private IEnumerator Flash(SpriteRenderer sr)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            sr.color = flashColor;
            yield return new WaitForSeconds(blinkInterval);
            sr.color = Color.white;
            yield return new WaitForSeconds(blinkInterval);
        }
        sr.color = Color.white;
        flashingSprite = null;
    }

    void OnDisable()
    {
        if (flashingSprite != null)
            flashingSprite.color = Color.white;
    }
}
