using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRecoveryAttackState : State
{
    protected D_EnemyRecoveryAttackState stateData;

    public EnemyRecoveryAttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyRecoveryAttackState stateData
    ) : base(entity, stateMachine, animBoolName)
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

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
