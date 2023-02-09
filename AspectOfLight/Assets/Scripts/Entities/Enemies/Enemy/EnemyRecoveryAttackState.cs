using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRecoveryAttackState : State
{
    protected D_EnemyRecoveryAttackState stateData;
    protected int MAX_ITERATIONS = 50;
    protected int totalIterationsMade = 0;

    public EnemyRecoveryAttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyRecoveryAttackState stateData
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        totalIterationsMade = 0;
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

    public virtual Vector2 GetNewRandomPosition(Vector2 center)
    {
        Vector2 _newRandomPosition =
            center + Random.insideUnitCircle * stateData.recoveryAreaRadius;

        return _newRandomPosition;
    }
}
