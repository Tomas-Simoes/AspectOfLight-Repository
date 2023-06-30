using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity, IDataPersistence
{
    public Player_MoveState moveState { get; private set; }
    public Player_AttackState attackState { get; private set; }
    public Player_IdleState idleState { get; private set; }
    public Player_CastingAbilityState castingAbilityState { get; private set; }

    [Header("States Data")]
    [SerializeField]
    private D_PlayerCoolDown coolDownData;

    [SerializeField]
    private D_PlayerMoveState moveStateData;

    [SerializeField]
    private D_PlayerAttackState attackStateData;

    public D_PlayerStamina staminaData;

    [Header("Cooldowns")]
    public float timeSinceAttack;
    public float timeSinceDash;
    public float timeSinceStaminaUsage;

    [Header("Player Information")]
    public Vector2 axisMovement;
    public float currentMovementSpeed;

    [Header("Stamina Information")]
    public bool hasStamina;
    public int currentStamina;

    [Header("The Plague Information")]
    public GameObject[] plagueSpawnPoints;

    [Header("Attack Information")]
    [SerializeField]
    public GameObject bulletPrefab;

    [SerializeField]
    public GameObject abilityInstances; //TODO change this to AbilitiesManager

    [SerializeField]
    public GameObject attackPosition;

    [Header("Stacks Information")]
    private int poisonStack = 0;
    public InputHandler inputHandler { get; private set; }

    public override void Awake()
    {
        base.Awake();

        idleState = new Player_IdleState(this, stateMachine, "idle", this);
        moveState = new Player_MoveState(this, stateMachine, "move", moveStateData, this);
        castingAbilityState = new Player_CastingAbilityState(this, stateMachine, "none", null);

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
        EventManager.StartListening("onPlayerCastingState", ChangeToCastAbilityState);
        EventManager.StartListening("onRest", Rest);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onRest", moveState.ChangePlayerMovementSpeed);
        EventManager.StopListening("onPlayerCastingState", ChangeToCastAbilityState);
        EventManager.StopListening("onRest", Rest);
    }

    public override void Start()
    {
        base.Start();
        GameManager.Instance.SavePlayerReference(this);

        StartCoroutine(RecoverStamina());

        inputHandler = this.GetComponent<InputHandler>();
        GetInputs();

        timeSinceDash = coolDownData.dashCD; //TODO remove this after transforming dash into an ability

        if (GameManager.Instance.environment == "development")
        {
            DataPersistenceManager.Instance.Init();
        }

        stateMachine.Init(idleState);
    }

    public override void Update()
    {
        base.Update();

        GetInputs();
        //RotatePlayerBasedOnMousePosition();
        CheckIfFlip(facingDirection, rb.position, rb.position + axisMovement);
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

        bool _isCastingAnotherAbility = castingAbilityState.currentAbility != null;

        bool _canAttack =
            inputHandler.pressingAttack
            && timeSinceAttack >= coolDownData.attackCD
            && stateMachine.currentState != attackState
            && HasStaminaToUse(attackStateData.staminaCost)
            && !_isCastingAnotherAbility;

        if (_canAttack)
            stateMachine.ChangeState(attackState);

        if (inputHandler.pressedAbilityOne)
            EventManager.TriggerEvent(
                "onAbilityCast",
                false,
                new Dictionary<string, object>
                {
                    { "slot", 1 },
                    { "currentStamina", currentStamina },
                    { "player", this },
                    { "abilityGettingCasted", castingAbilityState.currentAbility }
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
                    { "player", this },
                    { "abilityGettingCasted", castingAbilityState.currentAbility }
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
                    { "player", this },
                    { "abilityGettingCasted", castingAbilityState.currentAbility }
                }
            );

        if (inputHandler.pressedAbilityFour)
            EventManager.TriggerEvent(
                "onAbilityCast",
                false,
                new Dictionary<string, object>
                {
                    { "slot", 4 },
                    { "currentStamina", currentStamina },
                    { "player", this },
                    { "abilityGettingCasted", castingAbilityState.currentAbility }
                }
            );
    }

    public override void TakeDamage(float damage, Color damageColor)
    {
        if (GameManager.Instance.onGodMode)
            return;

        base.TakeDamage(damage, damageColor);

        EventManager.TriggerEvent("onCameraShake", false, null);
    }

    public override void TakePoisonDamage(float damage, float poisonTime, float tickRate)
    {
        poisonStack++;

        EventManager.TriggerEvent(
            "onUpdateStackBar",
            true,
            new Dictionary<string, object> { { "type", "poison" }, { "stackNumber", poisonStack } }
        );

        Coroutine poisonCoroutine = StartCoroutine(Poisoned(damage, tickRate));

        StartCoroutine(
            StopCoroutineAfterTime(
                poisonCoroutine,
                poisonTime,
                () =>
                {
                    poisonStack--;

                    EventManager.TriggerEvent(
                        "onUpdateStackBar",
                        true,
                        new Dictionary<string, object>
                        {
                            { "type", "poison" },
                            { "stackNumber", poisonStack }
                        }
                    );
                }
            )
        );
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

    public void ChangeToCastAbilityState(Dictionary<string, object> message)
    {
        if ((bool)message["activeState"])
        {
            castingAbilityState = new Player_CastingAbilityState(
                this,
                stateMachine,
                (string)message["animName"],
                message["ability"] as Ability
            );

            stateMachine.ChangeState(castingAbilityState);
        }
        else
        {
            stateMachine.ChangeState(idleState);
        }
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
            if (timeSinceStaminaUsage >= 2f && currentStamina <= staminaData.MAX_STAMINA)
            {
                currentStamina += staminaData.recoverStaminaRate;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public bool HasStaminaToUse(float staminaCost)
    {
        if (GameManager.Instance.onGodMode)
            return true;

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

    /*
    ! NOT IN USE
    ? Rotate Player based on mouse position instead of keyboard input.
    private void RotatePlayerBasedOnMousePosition()
    {
        Vector2 mouseDirection =
            Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

        bool _hasToFlip =
            mouseDirection.x > 0 && facingDirection == -1
            || mouseDirection.x < 0 && facingDirection == 1;

        if (_hasToFlip)
            Flip();
    }*/
}
