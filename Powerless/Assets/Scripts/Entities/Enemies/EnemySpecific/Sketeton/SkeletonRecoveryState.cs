using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonRecoveryState : EnemyRecoveryAttackState
{
    private Skeleton enemy;

    private D_EnemyRangedAttackState attackData;
    private D_EnemyCoolDown coolDownData;
    private D_EnemyPatrolState patrolStateData;

    public SkeletonRecoveryState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyRecoveryAttackState stateData,
        Skeleton enemy,
        D_EnemyRangedAttackState attackData,
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

        entity.SetVelocity(0f);
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
