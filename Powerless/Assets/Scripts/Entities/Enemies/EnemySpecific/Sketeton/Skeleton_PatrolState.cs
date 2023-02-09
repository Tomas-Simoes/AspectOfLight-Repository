using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_PatrolState : EnemyPatrolState
{
    private Skeleton enemy;
    private GameObject _currentPatrolPoint;

    public Skeleton_PatrolState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyPatrolState stateData,
        Skeleton enemy
    )
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.target = enemy.patrolPoints[currentPatrolIndex];

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
        currentPatrolPoint = enemy.patrolPoints[currentPatrolIndex];

        if (enemy.path != null && enemy.currentWaypoint >= enemy.path.vectorPath.Count)
        {
            int _newIndex = (currentPatrolIndex + 1) % enemy.patrolPoints.Length;
            currentPatrolIndex = _newIndex;

            enemy.target = enemy.patrolPoints[currentPatrolIndex];
            stateMachine.ChangeState(enemy.idleState);
        }
        else
        {
            enemy.goTowardsOtherPatrolPoint = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (enemy.path != null && enemy.currentWaypoint < enemy.path.vectorPath.Count)
        {
            Vector2 _direction =
                enemy.path.vectorPath[enemy.currentWaypoint] - enemy.transform.position;

            entity.rb.velocity = _direction.normalized * stateData.movementSpeed;
        }
    }
}
