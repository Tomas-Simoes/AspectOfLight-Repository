using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ChaseState : State
{
    protected D_EnemyChaseState stateData;

    public Enemy_ChaseState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyChaseState stateData
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
