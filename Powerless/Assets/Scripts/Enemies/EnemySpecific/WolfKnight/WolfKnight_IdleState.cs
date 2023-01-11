using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight_IdleState : EnemyIdleState
{
    private WolfKnight enemy;

    public WolfKnight_IdleState(
        Entity entity,
        EnemyStateMachine stateMachine,
        string animBoolName,
        D_EnemyIdleState stateData,
        WolfKnight enemy
    ) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isIdleTimeOver)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
