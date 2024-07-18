using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //Referencia ao script principal
    PlayerController playerMovementScript;

    private void Start()
    {
        //Pega a referencia ao script Principal
        playerMovementScript = GetComponent<PlayerController>();
    }
 
    public void OnMove(InputAction.CallbackContext moveInput)
    {
        //Pega o input do player e passa pro script principal
        Vector2 moveVector = moveInput.ReadValue<Vector2>();
        playerMovementScript.moveInputVector = moveVector;
    }

    public void OnShoot(InputAction.CallbackContext shootInput) 
    {
        if (shootInput.canceled) 
        {
            playerMovementScript.shouldShoot = false;
            return;
        }
        Vector2 shootVector = shootInput.ReadValue<Vector2>();
        float xComponent = shootVector.x;
        float yComponent = shootVector.y;
        if (Mathf.Abs(xComponent) > 0.5f || Mathf.Abs(yComponent) > 0.5f)
        {
            playerMovementScript.shootVector = shootVector;
            playerMovementScript.shouldShoot = true;
        }
    }

    public void OnAssimilation(InputAction.CallbackContext assimilateInput)
    {
        playerMovementScript.Assimilate();
    }

    public void OnItemChange(InputAction.CallbackContext itemInput) 
    {
        if (itemInput.started) 
        {
            playerMovementScript.ChangeItemSelected();
        }
    }

    public void UseItem(InputAction.CallbackContext itemInput)
    {
        if (itemInput.started)
        {
            playerMovementScript.UseItem();
        }
    }

    public void OnPause(InputAction.CallbackContext pause)
    {
        if (pause.started)
        {
            playerMovementScript.TogglePause();
        }
    }
}
