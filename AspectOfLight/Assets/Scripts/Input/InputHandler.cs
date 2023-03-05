using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputHandler : MonoBehaviour
{
    InputActions inputActions;

    [Header("Walking Input")]
    public float xAxis;
    public float yAxis;

    [Header("Abilities Input")]
    public bool pressingAttack;
    public bool pressedDash;
    public bool pressedAbilityOne; //? now it's the necromancer ability
    public bool pressedAbilityTwo;
    public bool pressedAbilityThree;

    [Header("Pause Input")]
    [SerializeField]
    PauseMenu pauseMenuHandler;

    [Header("UI Input")]
    [SerializeField]
    public bool pressedSelect;
    public bool pressedInteract;

    void Awake()
    {
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.XAxisMovement.performed += SetXAxisMovement;
        inputActions.Player.XAxisMovement.canceled += SetXAxisMovement;

        inputActions.Player.YAxisMovement.performed += SetYAxisMovement;
        inputActions.Player.YAxisMovement.canceled += SetYAxisMovement;

        inputActions.Player.Dash.started += SetDashing;
        inputActions.Player.Dash.canceled += SetDashing;

        inputActions.Player.AbilityOne.started += SetAbilityOne;
        inputActions.Player.AbilityOne.canceled += SetAbilityOne;

        inputActions.Player.AbilityTwo.started += SetAbilityTwo;
        inputActions.Player.AbilityTwo.canceled += SetAbilityTwo;

        inputActions.Player.AbilityThree.started += SetAbilityThree;
        inputActions.Player.AbilityThree.canceled += SetAbilityThree;

        inputActions.Player.Attack.performed += SetAttack;
        inputActions.Player.Attack.canceled += SetAttack;

        inputActions.Player.Interact.started += SetInteract;
        inputActions.Player.Interact.canceled += SetInteract;

        inputActions.UI.Pause.performed += SetPause;

        inputActions.UI.Select.started += SetSelect;
        inputActions.UI.Select.canceled += SetSelect;
    }

    private void OnDisable()
    {
        inputActions.Player.XAxisMovement.performed -= SetXAxisMovement;
        inputActions.Player.XAxisMovement.canceled -= SetXAxisMovement;

        inputActions.Player.XAxisMovement.performed -= SetYAxisMovement;
        inputActions.Player.XAxisMovement.canceled -= SetYAxisMovement;

        inputActions.Player.Dash.started -= SetDashing;
        inputActions.Player.Dash.canceled -= SetDashing;

        inputActions.Player.AbilityOne.started -= SetAbilityOne;
        inputActions.Player.AbilityOne.canceled += SetAbilityOne;

        inputActions.Player.Attack.performed -= SetAttack;
        inputActions.Player.Attack.canceled -= SetAttack;

        inputActions.UI.Pause.performed -= SetPause;

        inputActions.UI.Select.started -= SetSelect;
        inputActions.UI.Select.canceled += SetSelect;

        inputActions.Disable();
    }

    private void SetXAxisMovement(InputAction.CallbackContext ctx)
    {
        xAxis = ctx.ReadValue<Vector2>().x;
    }

    private void SetYAxisMovement(InputAction.CallbackContext ctx)
    {
        yAxis = ctx.ReadValue<Vector2>().y;
    }

    private void SetDashing(InputAction.CallbackContext ctx)
    {
        pressedDash = ctx.started;
    }

    private void SetAbilityOne(InputAction.CallbackContext ctx)
    {
        pressedAbilityOne = ctx.started;
    }

    private void SetAbilityTwo(InputAction.CallbackContext ctx)
    {
        pressedAbilityTwo = ctx.started;
    }

    private void SetAbilityThree(InputAction.CallbackContext ctx)
    {
        pressedAbilityThree = ctx.started;
    }

    private void SetAttack(InputAction.CallbackContext ctx)
    {
        pressingAttack = ctx.performed;
    }

    private void SetInteract(InputAction.CallbackContext ctx)
    {
        pressedInteract = ctx.started;
    }

    private void SetPause(InputAction.CallbackContext ctx)
    {
        if (!pauseMenuHandler.isPaused)
        {
            pauseMenuHandler.PauseGame();
        }
        else
        {
            pauseMenuHandler.ResumeGame();
        }
    }

    private void SetSelect(InputAction.CallbackContext ctx)
    {
        pressedSelect = ctx.started;
    }
}
