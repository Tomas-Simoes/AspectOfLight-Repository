using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornbornePlant_GrowingState : State
{
    ThornbornePlant enemy;
    float animationTime;

    public ThornbornePlant_GrowingState(
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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        animationTime = entity.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (animationTime >= 1)
            stateMachine.ChangeState(enemy.shootingState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
