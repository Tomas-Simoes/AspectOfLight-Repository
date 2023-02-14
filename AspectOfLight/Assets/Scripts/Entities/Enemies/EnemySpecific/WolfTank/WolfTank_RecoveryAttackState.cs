using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfTank_RecoveryAttackState : Enemy_RecoveryAttackState
{
    private WolfTank enemyWolfTank;
    private D_EnemyCoolDown coolDownData;

    public WolfTank_RecoveryAttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        WolfTank enemy,
        D_EnemyCoolDown coolDownData
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.enemyWolfTank = enemy;
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

        if (enemyWolfTank.timeSinceAttack >= coolDownData.normalAttackCD)
            stateMachine.ChangeState(enemyWolfTank.chaseState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
