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
    public bool pressedAbilityOne; 
    public bool pressedAbilityTwo;
    public bool pressedAbilityThree;

    [Header("Pause Input")]
    private bool isPaused = false;

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
        SubscribeInputs();

        EventManager.StartListening("onPlayerDeath", ChangeMapStatesBasedOnUI);
        EventManager.StartListening("onDialogue", ChangeMapStatesBasedOnUI);
        EventManager.StartListening("onPause", ChangeMapStatesBasedOnUI);
        EventManager.StartListening("onAbilityTree", ChangeMapStatesBasedOnUI);
    }

    private void OnDisable()
    {
        UnsubscribeInputs();

        EventManager.StopListening("onPlayerDeath", ChangeMapStatesBasedOnUI);
        EventManager.StopListening("onDialogue", ChangeMapStatesBasedOnUI);
        EventManager.StopListening("onPause", ChangeMapStatesBasedOnUI);
        EventManager.StopListening("onAbilityTree", ChangeMapStatesBasedOnUI);
    }

    private void SubscribeInputs()
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

        inputActions.InGameActions.Pause.started += SetPause;

        inputActions.InGameActions.Interact.performed += SetInteract;
        inputActions.InGameActions.Interact.canceled += SetInteract;

        inputActions.UI.Select.performed += SetSelect;
        inputActions.UI.Cancel.performed += SetCancel;
    }

    private void UnsubscribeInputs()
    {
        inputActions.Player.XAxisMovement.performed -= SetXAxisMovement;
        inputActions.Player.XAxisMovement.canceled -= SetXAxisMovement;

        inputActions.Player.XAxisMovement.performed -= SetYAxisMovement;
        inputActions.Player.XAxisMovement.canceled -= SetYAxisMovement;

        inputActions.Player.Dash.started -= SetDashing;
        inputActions.Player.Dash.canceled -= SetDashing;

        inputActions.Player.AbilityOne.started -= SetAbilityOne;
        inputActions.Player.AbilityOne.canceled -= SetAbilityOne;

        inputActions.Player.AbilityTwo.started -= SetAbilityTwo;
        inputActions.Player.AbilityTwo.canceled -= SetAbilityTwo;

        inputActions.Player.AbilityThree.started -= SetAbilityThree;
        inputActions.Player.AbilityThree.canceled -= SetAbilityThree;

        inputActions.Player.Attack.performed -= SetAttack;
        inputActions.Player.Attack.canceled -= SetAttack;

        inputActions.InGameActions.Interact.performed -= SetInteract;
        inputActions.InGameActions.Pause.performed -= SetPause;

        inputActions.UI.Select.performed -= SetSelect;
        inputActions.UI.Cancel.performed -= SetCancel;

        inputActions.Disable();
    }

    public void ChangeMapStatesBasedOnUI(Dictionary<string, object> message)
    {
        if (message["activeState"] == null)
        {
            Debug.LogWarning("The current UI event doesn't have \"UIInputMapState\" property");
        }

        bool _UIInputMapState = (bool)message["activeState"];

        ChangeInputMapState(inputActions.Player, !_UIInputMapState);
        ChangeInputMapState(inputActions.UI, _UIInputMapState);
    }

    public void ChangeInputMapState(InputActionMap map, bool state)
    {
        if (state)
            map.Enable();
        else
            map.Disable();
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
        pressedInteract = ctx.performed;
    }

    private void SetPause(InputAction.CallbackContext ctx)
    {
        EventManager.TriggerEvent(
            "onPause",
            true,
            new Dictionary<string, object> { { "activeState", !isPaused } }
        );

        isPaused = !isPaused; //TODO change this information to another class
    }

    private void SetSelect(InputAction.CallbackContext ctx)
    {
        pressedSelect = ctx.started;
    }

    private void SetCancel(InputAction.CallbackContext ctx)
    {
        EventManager.TriggerEvent("onCancel", true, null);
    }
}
