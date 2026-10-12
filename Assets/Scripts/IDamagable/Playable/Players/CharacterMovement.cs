using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public PlayableCharacter currentCharacter;
    public float horizontalMovement;
    //public Rigidbody2D rb;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = currentCharacter.GetComponent<Rigidbody2D>();
        Debug.Log(rb);
    }

    // Update is called once per frame
    void Update()
    {
        currentCharacter.Movement(rb, horizontalMovement);
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
