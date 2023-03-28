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
    public float timeSinceAbilityOne;
    public float timeSinceAbilityTwo;
    public float timeSinceAbilityThree;
    public float timeSinceStaminaUsage;

    [Header("Information")]
    public Vector2 axisMovement;

    [SerializeField]
    private bool FacingRight = true; //TODO change the code that uses this to use the entity.facingRight instead

    [Header("Abilities Information")]
    public bool hasStamina;
    public int currentStamina;

    [SerializeField]
    public AbilityManager abilityManager;

    public GameObject[] plagueSpawnPoints;

    [SerializeField]
    public GameObject bulletPrefab;

    [SerializeField]
    public GameObject bulletsParent;

    [SerializeField]
    public GameObject attackPosition;

    [Header("UI Managers")]
    public DeathUIManager deathUIManager;

    public InputHandler inputHandler { get; private set; }

    public override void Start()
    {
        base.Start();
        currentHealth = entityData.MAX_HEALTH;
        timeSinceDash = coolDownData.dashCD;
        currentStamina = staminaData.MAX_STAMINA;

        StartCoroutine(RecoverStamina());

        if (inputHandler == null)
        {
            inputHandler = this.GetComponent<InputHandler>();
        }

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

        abilityManager.InitAbilities();
        stateMachine.Init(moveState);
    }

    public override void Update()
    {
        base.Update();

        GetInputs();
        CheckForFlipping();
    }

    public override void UpdateCooldowns()
    {
        base.UpdateCooldowns();

        timeSinceDash += Time.deltaTime;

        timeSinceAbilityOne += Time.deltaTime;
        timeSinceAbilityTwo += Time.deltaTime;
        timeSinceAbilityThree += Time.deltaTime;

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
        {
            abilityManager.CastAbility(1, timeSinceAbilityOne);
        }

        if (inputHandler.pressedAbilityTwo)
        {
            abilityManager.CastAbility(2, timeSinceAbilityTwo);
        }

        if (inputHandler.pressedAbilityThree)
        {
            abilityManager.CastAbility(3, timeSinceAbilityThree);
        }

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

    public override void Die()
    {
        base.Die();

        deathUIManager.UpdateDeathUI();
    }

    private void CheckForFlipping()
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
            if (timeSinceStaminaUsage >= 1f)
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
        Debug.Log(data.playerPosition);
        transform.position = data.playerPosition;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = transform.position;
    }
}
