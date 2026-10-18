using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem; // to be assigned in the inspector through Player
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;
    [SerializeField] private int currentHp;
    [SerializeField] private int oldHpValue;
    [SerializeField] private Image Heart1;
    [SerializeField] private Image Heart2;
    [SerializeField] private Image Heart3;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {   
        if (healthSystem == null) // making sure we got HealthSystem reference
            return;

        currentHp = healthSystem.CurrentHp; // reads the exposed value
        updateHeartSprites(currentHp); // initialize the UI
        oldHpValue = currentHp; // initialize the old value for comparison
    }
    // Update is called once per frame
    void Update()
    {   
        if (healthSystem == null) // making sure we got HealthSystem reference
            return;

        currentHp = healthSystem.CurrentHp;

        if (currentHp != oldHpValue) // comparing to see if HP has changed
        {
            updateHeartSprites(currentHp);
        }
       
        oldHpValue = currentHp; // updating old HP 

    }

    public void updateHeartSprites(int currentHp)
    {
        // if currentHp = 3, change nothing (hearts are full)
        // if currentHp = 2, change Heart3 to empty heart sprite
        // if currentHp = 1, change Heart3 and Heart 2 to empty heart sprites
        // if currentHp = 0, change all Hearts to empty heart sprites

        switch (currentHp)
        {
            case 3: // nothing changes meaning 3 hearts remain full
                Heart3.sprite = fullHeartSprite;
                Heart2.sprite = fullHeartSprite;
                Heart1.sprite = fullHeartSprite;
                break;
            case 2:
                Heart3.sprite = emptyHeartSprite;
                Heart2.sprite = fullHeartSprite;
                Heart1.sprite = fullHeartSprite;
                break;
            case 1:
                Heart3.sprite = emptyHeartSprite;
                Heart2.sprite = emptyHeartSprite;
                Heart1.sprite = fullHeartSprite;
                break;
            case 0:
                Heart3.sprite = emptyHeartSprite;
                Heart2.sprite = emptyHeartSprite;
                Heart1.sprite = emptyHeartSprite;
                break;
            default: 
                break;
        }
    }

}
