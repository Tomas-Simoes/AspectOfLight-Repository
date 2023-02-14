using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttackState : State
{
    protected D_EnemyMeleeAttackState stateData;
    protected bool attackedOnce = false;

    public EnemyMeleeAttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyMeleeAttackState stateData
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
