using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public PlayerMoveState moveState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerDefaultRangedAttackState shootingState { get; private set; }
    public PlayerDefaultMeleeAttackState meleeState { get; private set; }
    public PlayerNecromancerState necromancerState { get; private set; }

    [Header("States Data")]
    [SerializeField]
    private D_PlayerCoolDown coolDownData;

    [SerializeField]
    private D_PlayerMoveState moveStateData;

    [SerializeField]
    private D_PlayerDashState dashStateData;

    [SerializeField]
    private D_PlayerNecromancerState necromancerStateData;

    [SerializeField]
    private D_PlayerMeleeState meleeStateData;

    [SerializeField]
    private D_PlayerShootingState shootingStateData;

    [Header("Cooldowns")]
    public float timeSinceDash;
    public float timeSinceNecromancer;

    [Header("Information")]
    public Vector2 axisMovement;
    public GameObject[] dronesSpawnPoints;

    [SerializeField]
    private Transform Barrel;

    [SerializeField]
    private float fireRate;

    [SerializeField]
    private GameObject bullet;

    public InputHandler inputHandler { get; private set; }

    public override void Start()
    {
        base.Start();
        currentHealth = entityData.MAX_HEALTH;
        timeSinceDash = coolDownData.dashCD;
        timeSinceNecromancer = coolDownData.necromancerCD;

        if (inputHandler == null)
        {
            inputHandler = this.GetComponent<InputHandler>();
        }

        moveState = new PlayerMoveState(this, stateMachine, "move", moveStateData, this);
        dashState = new PlayerDashState(
            this,
            stateMachine,
            "dash",
            dashStateData,
            this,
            coolDownData
        );

        shootingState = new PlayerDefaultRangedAttackState(
            this,
            stateMachine,
            "shoot",
            shootingStateData,
            this
        );

        meleeState = new PlayerDefaultMeleeAttackState(
            this,
            stateMachine,
            "shoot",
            meleeStateData,
            this
        );

        necromancerState = new PlayerNecromancerState(
            this,
            stateMachine,
            "necromancer",
            necromancerStateData,
            this,
            coolDownData
        );
        stateMachine.Init(moveState);
    }

    public override void Update()
    {
        base.Update();
        GetInputs();
    }

    public override void UpdateCooldowns()
    {
        base.UpdateCooldowns();

        timeSinceDash += Time.deltaTime;
        timeSinceNecromancer += Time.deltaTime;
    }

    private void GetInputs()
    {
        axisMovement.x = inputHandler.xAxis;
        axisMovement.y = inputHandler.yAxis;

        if (inputHandler.pressedDash && timeSinceDash >= coolDownData.dashCD)
        {
            stateMachine.ChangeState(dashState);
        }

        if (inputHandler.pressedAbilityOne && timeSinceNecromancer >= coolDownData.necromancerCD)
        {
            stateMachine.ChangeState(necromancerState);
        }
    }

    public void bulletTest()
    {
        Instantiate(bullet, Barrel.transform.position, Barrel.transform.rotation);
    }
}
