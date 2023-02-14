using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight_RecoveryAttackState : Enemy_RecoveryAttackState
{
    private WolfKnight enemyWolfKnight;
    private D_EnemyMeleeAttackState attackData;
    private D_EnemyCoolDown coolDownData;

    public WolfKnight_RecoveryAttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        WolfKnight enemy,
        D_EnemyMeleeAttackState attackData,
        D_EnemyCoolDown coolDownData
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.enemyWolfKnight = enemy;
        this.attackData = attackData;
        this.coolDownData = coolDownData;
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

        if (enemyWolfKnight.timeSinceAttack >= coolDownData.normalAttackCD)
            stateMachine.ChangeState(enemyWolfKnight.chaseState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
