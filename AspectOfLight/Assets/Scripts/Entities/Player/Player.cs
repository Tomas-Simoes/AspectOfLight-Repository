using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
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

    [Header("Cooldowns")]
    public float timeSinceAttack;
    public float timeSinceDash;
    public float timeSinceAbilityOne;
    public float timeSinceAbilityTwo;
    public float timeSinceAbilityThree;

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

    [Header("UI Elements")]
    public GameObject DeathScreen; //X

    public InputHandler inputHandler { get; private set; }

    public override void Start()
    {
        base.Start();
        currentHealth = entityData.MAX_HEALTH;
        timeSinceDash = coolDownData.dashCD;
        currentStamina = entityData.MAX_STAMINA;

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
        DeathScreen.SetActive(true);
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
    }

    public bool HasStaminaToUse(float staminaCost)
    {
        return currentStamina - staminaCost >= 0;
    }
}
