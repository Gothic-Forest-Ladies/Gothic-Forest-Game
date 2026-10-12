using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    PlayableCharacter currentCharacter;
    private CharacterSwitcher characterSwitcher;   //CharacterSwitcher activeCharacter;
    public float horizontalMovement;
    //public Rigidbody2D rb;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        characterSwitcher = GetComponent<CharacterSwitcher>(); // get it from the Player

        currentCharacter = characterSwitcher.ActiveCharacter; // get the active character from the CharacterSwitcher script
        rb = currentCharacter.GetComponent<Rigidbody2D>(); // get its rb to change velocity

        Debug.Log(rb);
    }

    // Update is called once per frame
    void Update()
    {
        currentCharacter.Movement(rb, horizontalMovement); 
        // need to update current character when switching happens
        currentCharacter = characterSwitcher.ActiveCharacter; // get the active character from the CharacterSwitcher script (checks if it was switched-> it updated)
        rb = currentCharacter.GetComponent<Rigidbody2D>(); // get its rb to change velocity
    }

    // Reads the keyboard input from Unity's Input System, such as pressing left or right.
    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        //Debug.Log("Move callback: " + horizontalMovement);
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
            currentCharacter.Jump(rb);
    }
    
}
