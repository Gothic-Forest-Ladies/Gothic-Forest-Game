using UnityEngine;
using UnityEngine.UI;

/// HUD row of character portraits.
/// Locked: darkened inactive sprite + "?" overlay. Unlocked but inactive: gray.
/// Active: full-color sprite, slightly enlarged.
public class CharacterIconUI : MonoBehaviour
{
    [SerializeField] private CharacterSwitcher switcher; // assigned in the inspector through Player

    [Header("Icon Images")]
    [SerializeField] private Image bearIcon;
    [SerializeField] private Image owlIcon;
    [SerializeField] private Image hedgehogIcon;

    [Header("Sprites")]
    [SerializeField] private Sprite bearActiveSprite;   // no inactive art for the bear: gray tint instead
    [SerializeField] private Sprite owlActiveSprite;
    [SerializeField] private Sprite owlInactiveSprite;
    [SerializeField] private Sprite hedgehogActiveSprite;
    [SerializeField] private Sprite hedgehogInactiveSprite;

    [Header("Locked '?' Overlays")]
    [SerializeField] private GameObject owlLockedOverlay;
    [SerializeField] private GameObject hedgehogLockedOverlay;

    [Header("Switch-Ready Glow")]
    [SerializeField] private Image bearGlow;      // pulses behind every unlocked, non-active icon
    [SerializeField] private Image owlGlow;
    [SerializeField] private Image hedgehogGlow;
    [SerializeField] private Color glowColor = new Color(1f, 0.85f, 0.3f, 0.8f);
    [SerializeField] private float pulseSpeed = 1.2f; // pulses per second

    private static readonly Color LockedTint = new Color(0.25f, 0.25f, 0.25f, 1f);
    private static readonly Color InactiveTint = new Color(0.55f, 0.55f, 0.55f, 1f);
    private static readonly Vector3 ActiveScale = Vector3.one * 1.15f;

    private int oldIndex = -1;
    private bool oldBird;
    private bool oldMouse;
    private float owlUnlockTime = -10f;      // for the one-shot unlock flash
    private float hedgehogUnlockTime = -10f;

    void Start()
    {
        if (switcher == null) // makes the HUD safe to reuse as a prefab in other scenes
            switcher = FindAnyObjectByType<CharacterSwitcher>();

        Refresh();
    }

    void Update()
    {
        if (switcher == null)
            return;

        // HeartUI-style change detection poll
        if (switcher.CurrentIndex != oldIndex ||
            switcher.birdUnlocked != oldBird ||
            switcher.mouseUnlocked != oldMouse)
        {
            if (!oldBird && switcher.birdUnlocked)
                owlUnlockTime = Time.unscaledTime;
            if (!oldMouse && switcher.mouseUnlocked)
                hedgehogUnlockTime = Time.unscaledTime;

            Refresh();
        }

        // glow = "you can switch to this one" (unscaled time so it keeps pulsing on the win freeze)
        AnimateGlow(bearGlow, oldIndex != 0, -10f);
        AnimateGlow(owlGlow, oldBird && oldIndex != 1, owlUnlockTime);
        AnimateGlow(hedgehogGlow, oldMouse && oldIndex != 2, hedgehogUnlockTime);
    }

    private void AnimateGlow(Image glow, bool available, float unlockTime)
    {
        if (glow == null)
            return;

        // one-shot bright flash right after unlocking, fading over 0.8s
        float flash = Mathf.Clamp01(1f - (Time.unscaledTime - unlockTime) / 0.8f);

        if (!available && flash <= 0f)
        {
            glow.enabled = false;
            return;
        }

        glow.enabled = true;
        float pulse = 0.5f + 0.5f * Mathf.Sin(Time.unscaledTime * pulseSpeed * 2f * Mathf.PI);
        Color c = glowColor;
        c.a = Mathf.Max(available ? glowColor.a * (0.35f + 0.65f * pulse) : 0f, flash);
        glow.color = c;
        glow.transform.localScale = Vector3.one * (1f + 0.12f * pulse + 0.6f * flash);
    }

    private void Refresh()
    {
        if (switcher == null)
            return;

        oldIndex = switcher.CurrentIndex;
        oldBird = switcher.birdUnlocked;
        oldMouse = switcher.mouseUnlocked;

        // Bear is always unlocked
        ApplyState(bearIcon, bearActiveSprite, bearActiveSprite, null, true, oldIndex == 0);
        ApplyState(owlIcon, owlActiveSprite, owlInactiveSprite, owlLockedOverlay, oldBird, oldIndex == 1);
        ApplyState(hedgehogIcon, hedgehogActiveSprite, hedgehogInactiveSprite, hedgehogLockedOverlay, oldMouse, oldIndex == 2);
    }

    private void ApplyState(Image icon, Sprite activeSprite, Sprite inactiveSprite,
                            GameObject lockedOverlay, bool unlocked, bool isActive)
    {
        if (icon == null)
            return;

        if (lockedOverlay != null)
            lockedOverlay.SetActive(!unlocked);

        if (!unlocked)
        {
            icon.sprite = inactiveSprite;
            icon.color = LockedTint;
            icon.transform.localScale = Vector3.one;
        }
        else if (isActive)
        {
            icon.sprite = activeSprite;
            icon.color = Color.white;
            icon.transform.localScale = ActiveScale;
        }
        else
        {
            icon.sprite = inactiveSprite;
            // the bear has no inactive sprite, so gray the active one out
            icon.color = inactiveSprite == activeSprite ? InactiveTint : Color.white;
            icon.transform.localScale = Vector3.one;
        }
    }
}
