using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : State
{
    protected D_EnemyIdleState stateData;
    protected bool flipAfterIdle;
    protected bool isIdleTimeOver;
    protected float idleTime;

    public EnemyIdleState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyIdleState stateData
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        entity.rb.velocity = Vector2.zero;

        isIdleTimeOver = false;

        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }
}
