using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight_RecoveryAttackState : EnemyRecoveryAttackState
{
    private WolfKnight enemy;
    private D_EnemyMeleeAttackState attackData;
    private D_EnemyCoolDown coolDownData;
    private D_EnemyPatrolState patrolStateData;

    private bool goTowardsTarget;
    private bool goingTowardsRecoveryPoint;
    private Collider2D _collider;
    public Vector2 recoveryPointPosition;
    public bool firstTimeInState;

    public WolfKnight_RecoveryAttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyRecoveryAttackState stateData,
        WolfKnight enemy,
        D_EnemyMeleeAttackState attackData,
        D_EnemyCoolDown coolDownData,
        D_EnemyPatrolState patrolStateData
    )
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
        this.attackData = attackData;
        this.coolDownData = coolDownData;
        this.patrolStateData = patrolStateData;
    }

    public override void Enter()
    {
        base.Enter();

        entity.rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.timeSinceAttack >= coolDownData.normalAttackCD)
            stateMachine.ChangeState(enemy.chaseState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
