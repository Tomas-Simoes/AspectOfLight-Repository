using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfTank_ChaseState : EnemyChaseState
{
    private WolfTank enemy;
    private D_EnemyData enemyData;

    public WolfTank_ChaseState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyChaseState stateData,
        WolfTank enemy,
        D_EnemyData enemyData
    ) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemyData = enemyData;
        this.enemy = enemy;
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

        entity.CheckIfFlip(
            enemy.facingDirection,
            enemy.transform.position,
            enemy.target.transform.position
        );
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        StartChasing(enemy.target);
    }
}
