using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight_IdleState : Enemy_IdleState
{
    private WolfKnight enemyWolfKnight;

    public WolfKnight_IdleState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyIdleState stateData,
        WolfKnight enemy
    )
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemyWolfKnight = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        entity.CheckIfFlip(
            enemyWolfKnight.facingDirection,
            enemyWolfKnight.transform.position,
            enemyWolfKnight.target.transform.position
        );
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isIdleTimeOver)
        {
            stateMachine.ChangeState(enemyWolfKnight.patrolState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
