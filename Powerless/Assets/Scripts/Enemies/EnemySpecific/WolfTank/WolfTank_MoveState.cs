using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfTank_MoveState : EnemyMoveState
{
    private WolfTank enemy;
    private GameObject _currentPatrolPoint;

    public WolfTank_MoveState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyMoveState stateData,
        WolfTank enemy
    ) : base(entity, stateMachine, animBoolName, stateData)
    {
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
        _currentPatrolPoint = enemy.patrolPoints[currentPatrolIndex];

        if (
            Vector3.Distance(enemy.transform.position, _currentPatrolPoint.transform.position)
            < 0.5f
        )
        {
            int _newIndex = (currentPatrolIndex + 1) % enemy.patrolPoints.Length;

            SetCurrentPatrolIndex(_newIndex);
            entity.CheckIfFlip(
                enemy.facingDirection,
                _currentPatrolPoint.transform.position,
                enemy.patrolPoints[_newIndex].transform.position
            );

            stateMachine.ChangeState(enemy.idleState);
        }
        else
        {
            stateData.goTowardsOtherPatrolPoint = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (stateData.goTowardsOtherPatrolPoint && _currentPatrolPoint != null)
        {
            MoveTowardsPatrolPoint(_currentPatrolPoint, stateData.movementSpeed);
        }
    }
}
