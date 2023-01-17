using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : EnemyMoveState
{
  private Skeleton enemySkeleton;
  private GameObject _currentPatrolPoint;

  public SkeletonMoveState(
   Entity entity,
   StateMachine stateMachine,
   string animBoolName,
   D_EnemyMoveState stateData,
   Skeleton enemy,
   D_EnemyData enemyData
) : base(entity, stateMachine, animBoolName, stateData)
  {
    this.enemySkeleton = enemy;
  }

  public override void Enter()
  {
    base.Enter();
    if (_currentPatrolPoint != null)
    {
      entity.CheckIfFlip(
          enemySkeleton.facingDirection,
          enemySkeleton.transform.position,
          _currentPatrolPoint.transform.position
      );
    }
  }

  public override void Exit()
  {
    base.Exit();
  }

  public override void GetInputs()
  {
    base.GetInputs();
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();
    _currentPatrolPoint = enemySkeleton.patrolPoints[currentPatrolIndex];

    if (
        Vector3.Distance(enemySkeleton.transform.position, _currentPatrolPoint.transform.position)
        < 0.5f
    )
    {
      int _newIndex = (currentPatrolIndex + 1) % enemySkeleton.patrolPoints.Length;

      SetCurrentPatrolIndex(_newIndex);
      entity.CheckIfFlip(
          enemySkeleton.facingDirection,
          _currentPatrolPoint.transform.position,
          enemySkeleton.patrolPoints[_newIndex].transform.position
      );

      stateMachine.ChangeState(enemySkeleton.idleState);
    }
    else
    {
      stateData.goTowardsOtherPatrolPoint = true;
    }
  }

  public override void UpdateCooldowns()
  {
    base.UpdateCooldowns();
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
