using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : State
{
    protected D_EnemyPatrolState stateData;
    public int currentPatrolIndex = 0;

    public EnemyPatrolState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyPatrolState stateData
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(stateData.movementSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public void SetCurrentPatrolIndex(int newIndex)
    {
        currentPatrolIndex = newIndex;
    }
}
