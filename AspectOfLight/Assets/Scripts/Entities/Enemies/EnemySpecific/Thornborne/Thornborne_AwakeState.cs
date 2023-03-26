using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornborne_AwakeState : State
{
    //TODO Change this to work with animations

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
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        animationTime += Time.deltaTime;

        if (animationTime >= 2)
            stateMachine.ChangeState(enemy.chaseState);
    }
}
