using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyMoveState : EnemyState
{
    protected D_EnemyMoveState stateData;
    protected bool isDetectingWall;

    public EnemyMoveState(
        Entity entity,
        EnemyStateMachine stateMachine,
        string animBoolName,
        D_EnemyMoveState stateData
    ) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(stateData.movementSpeed);

        isDetectingWall = entity.CheckWall();
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
