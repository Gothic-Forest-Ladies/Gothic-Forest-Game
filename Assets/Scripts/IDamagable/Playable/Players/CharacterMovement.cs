using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    // added changes for the bird character, vertical movement and special ability (17.10.26, Ola)
    PlayableCharacter currentCharacter;
    private CharacterSwitcher characterSwitcher;   //CharacterSwitcher activeCharacter;
    public float horizontalMovement;
    public float verticalMovement; //Added vertical movement for the bird character
    //public Rigidbody2D rb;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        characterSwitcher = GetComponent<CharacterSwitcher>(); // get it from the Player

        currentCharacter = characterSwitcher.ActiveCharacter; // get the active character from the CharacterSwitcher script
        rb = currentCharacter.GetComponent<Rigidbody2D>(); // get its rb to change velocity

        // Debug.Log(rb);
    }

    // Update is called once per frame
    
    void Update()
{
    // Update the active character references first
    currentCharacter = characterSwitcher.ActiveCharacter;

    if (currentCharacter == null) // (checks if it was switched-> it updated)
        return;

    // get its rb to change velocity (in case it was switched)
    rb = currentCharacter.GetComponent<Rigidbody2D>();

    if (rb == null)
        return;

    currentCharacter.horizontalMovement = horizontalMovement;

    // Don't overwrite Rigidbody2D velocity during knockback.
    if (currentCharacter.IsKnockedBack)
        return;

    currentCharacter.Movement(rb, horizontalMovement, verticalMovement); //Added vertical movement for the bird character
}


    // Reads the keyboard input from Unity's Input System, such as pressing left or right.
    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        //Debug.Log("Move callback: " + horizontalMovement);
        verticalMovement = context.ReadValue<Vector2>().y; //Added vertical movement for the bird character
        //Debug.Log("Move callback: " + verticalMovement);
    }


    public void SpecialAbility(InputAction.CallbackContext context)
    {
        if (context.performed)
            currentCharacter.SpecialAbility();
    }
    
}
