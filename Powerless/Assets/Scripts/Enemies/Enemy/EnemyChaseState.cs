using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : State
{
    protected D_EnemyChaseState stateData;

    public EnemyChaseState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyChaseState stateData
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

    public bool CheckIfFlip(int facingDirection, Vector2 currentPosition, Vector2 futurePosition)
    {
        bool _newPatrolPointIsOnLeft = currentPosition.x > futurePosition.x;

        if (facingDirection == 1)
        {
            return _newPatrolPointIsOnLeft;
        }
        else
        {
            return !_newPatrolPointIsOnLeft;
        }
    }
}
