using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight_MoveState : EnemyMoveState
{
    private WolfKnight enemy;
    private GameObject _waypoint;

    public WolfKnight_MoveState(
        Entity entity,
        EnemyStateMachine stateMachine,
        string animBoolName,
        D_EnemyMoveState stateData,
        WolfKnight enemy
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
        _waypoint = enemy.patrolPoints[enemy.currentPatrolIndex];

       // Debug.Log(Vector3.Distance(enemy.transform.position, _waypoint.transform.position));

        if (Vector3.Distance(enemy.transform.position, _waypoint.transform.position) < 0.5f)
        {
            int _newIndex = (enemy.currentPatrolIndex + 1) % enemy.patrolPoints.Length;
            enemy.SetCurrentPatrolIndex(_newIndex);

            bool _newPatrolPointIsOnLeft =
                _waypoint.transform.position.x > enemy.patrolPoints[_newIndex].transform.position.x;

            if (enemy.facingDirection == 1)
            {
                if (_newPatrolPointIsOnLeft)
                {
                    enemy.idleState.SetFlipAfterIdle(true);
                }
            }
            else
            {
                if (!_newPatrolPointIsOnLeft)
                {
                    enemy.idleState.SetFlipAfterIdle(true);
                }
            }

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
        if (stateData.goTowardsOtherPatrolPoint && _waypoint != null)
        {
            enemy.MoveTowardsPatrolPoint(_waypoint, stateData.movementSpeed);
        }
    }
}
