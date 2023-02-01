using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : State
{
    protected D_EnemyPatrolState stateData;
    protected int currentPatrolIndex = 0;
    protected GameObject currentPatrolPoint;

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
    }

    public override void Exit()
    {
        base.Exit();
    }
}
