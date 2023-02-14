using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttackState : State
{
    protected D_EnemyRangedAttackState stateData;
    protected bool attackedOnce = false;

    public EnemyRangedAttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyRangedAttackState stateData
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        entity.rb.velocity = Vector2.zero;
        attackedOnce = false;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
