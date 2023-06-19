using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornbornePlant_IdleState : State
{
    ThornbornePlant enemy;

    private float timeSinceShooting;

    public ThornbornePlant_IdleState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        ThornbornePlant enemy
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        timeSinceShooting = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        timeSinceShooting += Time.deltaTime;

        if (timeSinceShooting >= enemy.shootingCD)
        {
            stateMachine.ChangeState(enemy.shootingState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
