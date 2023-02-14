using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRecoveryAttackState : State
{
    public EnemyRecoveryAttackState(Entity entity, StateMachine stateMachine, string animBoolName)
        : base(entity, stateMachine, animBoolName) { }

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
