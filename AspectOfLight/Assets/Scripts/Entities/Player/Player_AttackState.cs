using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Player_AttackState : State
{
    public Player player;
    public D_PlayerAttackState stateData;

    [Header("Attack Information")]
    public float attackTime;
    private float changeDamageTime;
    private float currentDamage;

    [Header("Stamina Information")]
    private float currentStaminaCostTime;

    public Player_AttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_PlayerAttackState stateData,
        Player player,
        D_PlayerCoolDown coolDownData
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.player = player;
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        attackTime = 0;
        currentStaminaCostTime = stateData.staminaCostTime;

        player.timeSinceAttack = 0;

        changeDamageTime = stateData.changeDamageTimeJump;
        currentDamage = stateData.defaultDamage;

        entity.rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (attackTime >= stateData.maxAttackTime)
        {
            currentStaminaCostTime += Time.deltaTime;

            if (currentStaminaCostTime >= stateData.staminaCostTime)
            {
                player.ReduceStamina(stateData.staminaCost);
                currentStaminaCostTime = 0;
            }
        }
        else
        {
            attackTime += Time.deltaTime;

            if (attackTime >= changeDamageTime)
            {
                changeDamageTime += stateData.changeDamageTimeJump;
                currentDamage += stateData.damageIncrement;
            }
        }

        bool _releaseAttack =
            !player.inputHandler.pressingAttack || !player.HasStaminaToUse(stateData.staminaCost);

        if (_releaseAttack)
        {
            GameObject _bullet = entity.CreatePrefab(
                player.bulletPrefab,
                player.attackPosition.transform.position,
                entity.abilityInstances
            );

            _bullet.GetComponent<DefaultBullet>().damage = GameManager.Instance.onGodMode
                ? 100
                : currentDamage;

            stateMachine.ChangeState(
                player.axisMovement == Vector2.zero ? player.idleState : player.moveState
            );
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
