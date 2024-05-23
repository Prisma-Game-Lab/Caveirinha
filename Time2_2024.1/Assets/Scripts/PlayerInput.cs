using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    //Referencia ao script principal
    PlayerMovement playerMovementScript;

    private void Start()
    {
        //Pega a referencia ao script Principal
        playerMovementScript = GetComponent<PlayerMovement>();
    }
 
    public void OnMove(InputAction.CallbackContext moveInput)
    {
        //Pega o input do player e passa pro script principal
        Vector2 moveVector = moveInput.ReadValue<Vector2>();
        playerMovementScript.moveInputVector = moveVector;
    }
}
