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
    public bool pressedDash;
    public bool pressedAbilityOne; //? now it's the necromancer ability

    public bool fireBowM2;

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

    private void SetfireBowM2(InputAction.CallbackContext ctx)
    {
        pressedAbilityOne = ctx.started;
    }
}
