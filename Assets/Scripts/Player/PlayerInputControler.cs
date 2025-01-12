using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputControler : MonoBehaviour
{
    PlayerInput playerInput;

    public InputAction changeCamera { get; private set; }
    public InputAction pauseGame { get; private set; }
    public InputAction editColor { get; private set; }
    public InputAction ignoreGrid { get; private set; }
    public InputAction deleteObj { get; private set; }
    public InputAction editObj { get; private set; }
    public InputAction place { get; private set; }
    public InputAction rotate { get; private set; }
    public InputAction look { get; private set; }
    public InputAction move { get; private set; }
    public InputAction jump { get; private set; }
    public InputAction openInventory { get; private set; }
    public InputAction enterEditMode { get; private set; }   
    
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        pauseGame = playerInput.actions["PauseGame"];

        changeCamera = playerInput.actions["SwapCameras"];

        //Movements
        look = playerInput.actions["Look"];
        move = playerInput.actions["Move"];
        jump = playerInput.actions["Jump"];

        editColor = playerInput.actions["EditColor"];
        ignoreGrid = playerInput.actions["IgnoreGrid"];
        deleteObj = playerInput.actions["DeleteObj"];
        editObj = playerInput.actions["EditObject"];
        place = playerInput.actions["Place"];
        rotate = playerInput.actions["Rotate"];

        openInventory = playerInput.actions["Inventory"];
        enterEditMode = playerInput.actions["EnterEditMode"];

        DisableInputs();
    }

    public void DisableInputs()
    {
        // Disable movement and related actions
        move.Disable();
        look.Disable();
        jump.Disable();
        place.Disable();
        rotate.Disable();

        // Disable editing actions
        editColor.Disable();
        ignoreGrid.Disable();
        deleteObj.Disable();
        editObj.Disable();

        changeCamera.Disable();

        // Disable mode switching
        enterEditMode.Disable();
        openInventory.Disable();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EnableInputs()
    {
        // Enable movement and related actions
        move.Enable();
        look.Enable();
        jump.Enable();
        place.Enable();
        rotate.Enable();

        changeCamera.Enable();

        // Enable editing actions
        editColor.Enable();
        ignoreGrid.Enable();
        deleteObj.Enable();
        editObj.Enable();

        // Enable mode switching
        enterEditMode.Enable();
        openInventory.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DisableInputsOnInvenotryInputs()
    {
        // Disable movement and related actions
        move.Disable();
        look.Disable();
        jump.Disable();
        place.Disable();
        rotate.Disable();

        changeCamera.Disable();

        // Disable editing actions
        editColor.Disable();
        ignoreGrid.Disable();
        deleteObj.Disable();
        editObj.Disable();

        // Disable mode switching
        enterEditMode.Disable();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DisableInputsOnColorPickerInputs()
    {
        // Disable movement and related actions
        move.Disable();
        look.Disable();
        jump.Disable();
        place.Disable();
        rotate.Disable();

        changeCamera.Disable();

        // Disable editing actions
        ignoreGrid.Disable();
        deleteObj.Disable();
        editObj.Disable();

        // Disable mode switching
        enterEditMode.Disable();
        openInventory.Disable();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}


