using UnityEngine;

public class WolfTank_PatrolState : Enemy_PatrolState
{
    private WolfTank enemyWolfTank;

    public WolfTank_PatrolState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyPatrolState stateData,
        WolfTank enemy
    )
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemyWolfTank = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemyWolfTank.target = enemyWolfTank.patrolPoints[currentPatrolIndex];

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
        currentPatrolPoint = enemyWolfTank.patrolPoints[currentPatrolIndex];

        if (enemyWolfTank.path != null && enemyWolfTank.currentWaypoint >= enemyWolfTank.path.vectorPath.Count)
        {
            int _newIndex = (currentPatrolIndex + 1) % enemyWolfTank.patrolPoints.Length;
            currentPatrolIndex = _newIndex;

            enemyWolfTank.target = enemyWolfTank.patrolPoints[currentPatrolIndex];
            stateMachine.ChangeState(enemyWolfTank.idleState);
        }
        else
        {
            enemyWolfTank.goTowardsOtherPatrolPoint = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (enemyWolfTank.path != null && enemyWolfTank.currentWaypoint < enemyWolfTank.path.vectorPath.Count)
        {
            Vector2 _direction =
                enemyWolfTank.path.vectorPath[enemyWolfTank.currentWaypoint] - enemyWolfTank.transform.position;

            entity.rb.velocity = _direction.normalized * stateData.movementSpeed;
        }
    }
}
