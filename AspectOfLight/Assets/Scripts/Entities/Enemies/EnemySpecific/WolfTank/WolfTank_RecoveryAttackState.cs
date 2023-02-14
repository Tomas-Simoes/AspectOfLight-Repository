using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfTank_RecoveryAttackState : EnemyRecoveryAttackState
{
    private WolfTank enemy;
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
        this.enemy = enemy;
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

        if (enemy.timeSinceAttack >= coolDownData.normalAttackCD)
            stateMachine.ChangeState(enemy.chaseState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
