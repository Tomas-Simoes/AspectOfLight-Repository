using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight_PatrolState : EnemyPatrolState
{
    private WolfKnight enemy;

    public WolfKnight_PatrolState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyPatrolState stateData,
        WolfKnight enemy
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
            enemy.facingDirection,
            enemy.transform.position,
            enemy.target.transform.position
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

        if (enemy.currentWaypoint >= enemy.path.vectorPath.Count)
        {
            Debug.Log("Current path waypoint count" + enemy.path.vectorPath.Count);
            Debug.Log("Change Target");
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
        if (enemy.goTowardsOtherPatrolPoint && currentPatrolPoint != null)
        {
            Vector2 _direction =
                enemy.path.vectorPath[enemy.currentWaypoint] - enemy.transform.position;

            entity.rb.velocity = _direction.normalized * stateData.movementSpeed;
        }
    }
}
