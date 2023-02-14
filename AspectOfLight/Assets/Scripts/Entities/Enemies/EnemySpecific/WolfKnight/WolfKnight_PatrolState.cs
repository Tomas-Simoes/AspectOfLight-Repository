using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight_PatrolState : Enemy_PatrolState
{
    private WolfKnight enemyWolfKnight;

    public WolfKnight_PatrolState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyPatrolState stateData,
        WolfKnight enemy
    )
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemyWolfKnight = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemyWolfKnight.target = enemyWolfKnight.patrolPoints[currentPatrolIndex];

        entity.CheckIfFlip(
            entity.facingDirection,
            entity.transform.position,
            entity.target.transform.position
        );
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        currentPatrolPoint = enemyWolfKnight.patrolPoints[currentPatrolIndex];

        if (enemyWolfKnight.path != null && enemyWolfKnight.currentWaypoint >= enemyWolfKnight.path.vectorPath.Count)
        {
            int _newIndex = (currentPatrolIndex + 1) % enemyWolfKnight.patrolPoints.Length;
            currentPatrolIndex = _newIndex;

            enemyWolfKnight.target = enemyWolfKnight.patrolPoints[currentPatrolIndex];
            stateMachine.ChangeState(enemyWolfKnight.idleState);
        }
        else
        {
            enemyWolfKnight.goTowardsOtherPatrolPoint = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (enemyWolfKnight.path != null && enemyWolfKnight.currentWaypoint < enemyWolfKnight.path.vectorPath.Count)
        {
            Vector2 _direction =
                enemyWolfKnight.path.vectorPath[enemyWolfKnight.currentWaypoint] - enemyWolfKnight.transform.position;

            entity.rb.velocity = _direction.normalized * stateData.movementSpeed;
        }
    }
}
