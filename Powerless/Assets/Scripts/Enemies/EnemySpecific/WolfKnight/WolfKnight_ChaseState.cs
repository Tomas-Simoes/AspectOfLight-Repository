using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight_ChaseState : EnemyChaseState
{
    private WolfKnight enemy;
    private D_EnemyData enemyData;

    public WolfKnight_ChaseState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyChaseState stateData,
        WolfKnight enemy,
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
            enemyData.target.transform.position
        );
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        StartChasing(enemyData.target);
    }
}
