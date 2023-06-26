using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornborne_AwakeState : State
{
    private Thornborne enemy;
    private float animationTime;

    public Thornborne_AwakeState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        Thornborne enemy
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

        enemy.canTakeDamage = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        animationTime = entity.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (animationTime >= 1)
            stateMachine.ChangeState(enemy.chaseState);
    }
}
