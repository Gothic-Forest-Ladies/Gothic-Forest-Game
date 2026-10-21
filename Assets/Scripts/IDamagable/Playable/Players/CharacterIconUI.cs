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

    [Header("Unlocked Glow")]
    [SerializeField] private Image bearGlow;      // steady soft glow behind every unlocked icon
    [SerializeField] private Image owlGlow;
    [SerializeField] private Image hedgehogGlow;
    [SerializeField] private Color glowColor = new Color(1f, 0.85f, 0.3f, 0.5f);

    private static readonly Color LockedTint = new Color(0.25f, 0.25f, 0.25f, 1f);
    private static readonly Color InactiveTint = new Color(0.55f, 0.55f, 0.55f, 1f);
    private static readonly Vector3 ActiveScale = Vector3.one * 1.15f;

    private int oldIndex = -1;
    private bool oldBird;
    private bool oldMouse;

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
            Refresh();
        }
    }

    private void SetGlow(Image glow, bool unlocked)
    {
        if (glow == null)
            return;

        glow.enabled = unlocked;
        glow.color = glowColor;
        glow.transform.localScale = Vector3.one;
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

        SetGlow(bearGlow, true);
        SetGlow(owlGlow, oldBird);
        SetGlow(hedgehogGlow, oldMouse);
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
