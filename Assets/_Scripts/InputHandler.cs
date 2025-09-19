using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{

    InputSystem_Actions playerInput;

    private void Awake()
    {
        playerInput = new InputSystem_Actions();
        playerInput.Player.Enable();

        playerInput.Player.Jump.performed += Jump;
        playerInput.Player.Attack.performed += Attack; 
        playerInput.Player.Dash.performed += Dash;
    }
    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInput.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {

            PlayerController.instance.HandleJump();
        }
    }

    void Attack(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            PlayerController.instance.Attack();
        }
    }

    void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PlayerController.instance.Dash();

        }
    }
}

