using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornbornePlant_ShootingState : State
{
    ThornbornePlant enemy;

    public ThornbornePlant_ShootingState(
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
        enemy.StartShooting();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
