using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity, IDataPersistence
{
    public Player_MoveState moveState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_AttackState attackState { get; private set; }

    [Header("States Data")]
    [SerializeField]
    private D_PlayerCoolDown coolDownData;

    [SerializeField]
    private D_PlayerMoveState moveStateData;

    [SerializeField]
    private D_PlayerDashState dashStateData;

    [SerializeField]
    private D_PlayerAttackState attackStateData;

    public D_PlayerStamina staminaData;

    [Header("Cooldowns")]
    public float timeSinceAttack;
    public float timeSinceDash;
    public float timeSinceStaminaUsage;

    [Header("Information")]
    public Vector2 axisMovement;
    public float currentMovementSpeed;

    [Header("Abilities Information")]
    public bool hasStamina;
    public int currentStamina;
    public GameObject[] plagueSpawnPoints;

    [SerializeField]
    public GameObject bulletPrefab;

    [SerializeField]
    public GameObject bulletsParent; //TODO change this to AbilitiesManager

    [SerializeField]
    public GameObject attackPosition;

    public InputHandler inputHandler { get; private set; }

    public override void Awake()
    {
        base.Awake();

        moveState = new Player_MoveState(this, stateMachine, "move", moveStateData, this);
        dashState = new Player_DashState(
            this,
            stateMachine,
            "dash",
            dashStateData,
            this,
            coolDownData
        );

        attackState = new Player_AttackState(
            this,
            stateMachine,
            "attack",
            attackStateData,
            this,
            coolDownData
        );

        AbilityManager.Instance.Init();
        CameraManager.Instance.Init();
    }

    private void OnEnable()
    {
        EventManager.StartListening("onRest", moveState.ChangePlayerMovementSpeed);
        EventManager.StartListening("onRest", Rest);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onRest", moveState.ChangePlayerMovementSpeed);
        EventManager.StopListening("onRest", Rest);
    }

    public override void Start()
    {
        base.Start();
        timeSinceDash = coolDownData.dashCD;

        StartCoroutine(RecoverStamina());

        inputHandler = this.GetComponent<InputHandler>();

        //! THIS IS FOR DEVELOPMENT ONLY
        //! REMOVE THIS WHEN BUILDING THE GAME
        DataPersistenceManager.Instance.Init();

        GameManager.Instance.SavePlayerReference(this);

        stateMachine.Init(moveState);
    }

    public override void Update()
    {
        base.Update();

        GetInputs();
        RotatePlayerBasedOnMousePosition();
    }

    public override void UpdateCooldowns()
    {
        base.UpdateCooldowns();

        timeSinceDash += Time.deltaTime;
        timeSinceAttack += Time.deltaTime;
        timeSinceStaminaUsage += Time.deltaTime;
    }

    private void GetInputs()
    {
        axisMovement.x = inputHandler.xAxis;
        axisMovement.y = inputHandler.yAxis;

        if (
            inputHandler.pressedDash
            && timeSinceDash >= coolDownData.dashCD
            && HasStaminaToUse(dashStateData.staminaCost)
        )
        {
            stateMachine.ChangeState(dashState);
        }

        if (inputHandler.pressedAbilityOne)
            EventManager.TriggerEvent(
                "onAbilityCast",
                false,
                new Dictionary<string, object>
                {
                    { "slot", 1 },
                    { "currentStamina", currentStamina },
                    { "player", this }
                }
            );

        if (inputHandler.pressedAbilityTwo)
            EventManager.TriggerEvent(
                "onAbilityCast",
                false,
                new Dictionary<string, object>
                {
                    { "slot", 2 },
                    { "currentStamina", currentStamina },
                    { "player", this }
                }
            );

        if (inputHandler.pressedAbilityThree)
            EventManager.TriggerEvent(
                "onAbilityCast",
                false,
                new Dictionary<string, object>
                {
                    { "slot", 3 },
                    { "currentStamina", currentStamina },
                    { "player", this }
                }
            );

        if (
            inputHandler.pressingAttack
            && timeSinceAttack >= coolDownData.attackCD
            && stateMachine.currentState != attackState
            && HasStaminaToUse(attackStateData.staminaCost)
        )
        {
            stateMachine.ChangeState(attackState);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        EventManager.TriggerEvent("onCameraShake", false, null);
    }

    public override void Die()
    {
        EventManager.TriggerEvent(
            "onPlayerDeath",
            true,
            new Dictionary<string, object> { { "activeState", true } }
        );
    }

    public void Rest(Dictionary<string, object> message)
    {
        currentHealth = entityData.MAX_HEALTH;
        currentStamina = staminaData.MAX_STAMINA;
    }

    private void RotatePlayerBasedOnMousePosition()
    {
        Vector2 mouseDirection =
            Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

        bool _hasToFlip =
            mouseDirection.x > 0 && facingDirection == -1
            || mouseDirection.x < 0 && facingDirection == 1;

        if (_hasToFlip)
            Flip();
    }

    public void ReduceStamina(float stamina)
    {
        currentStamina -= Mathf.FloorToInt(stamina);

        hasStamina = currentStamina <= 0 ? false : true;
        timeSinceStaminaUsage = 0;
    }

    private IEnumerator RecoverStamina()
    {
        while (true)
        {
            if (timeSinceStaminaUsage >= 1f || currentStamina <= staminaData.MAX_STAMINA)
            {
                currentStamina += staminaData.recoverStaminaRate;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public bool HasStaminaToUse(float staminaCost)
    {
        return currentStamina - staminaCost >= 0;
    }

    public void LoadData(GameData data)
    {
        currentHealth = data.currentHealth;
        currentStamina = data.currentStamina;
        transform.position = data.playerPosition;
        currentMovementSpeed = data.movementSpeed;
    }

    public void SaveData(GameData data)
    {
        data.currentHealth = currentHealth;
        data.currentStamina = currentStamina;
        data.movementSpeed = currentMovementSpeed;
        data.playerPosition = transform.position;
    }
}
