using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight_MoveState : EnemyMoveState
{
  private WolfKnight enemy;
  private GameObject _waypoint;

  public WolfKnight_MoveState(
      Entity entity,
      StateMachine stateMachine,
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
    _waypoint = enemy.patrolPoints[currentPatrolIndex];

    if (Vector3.Distance(enemy.transform.position, _waypoint.transform.position) < 0.5f)
    {
      int _newIndex = (currentPatrolIndex + 1) % enemy.patrolPoints.Length;
      SetCurrentPatrolIndex(_newIndex);
      bool _newPatrolPointIsOnLeft =
          _waypoint.transform.position.x > enemy.patrolPoints[_newIndex].transform.position.x;


      if (enemy.facingDirection == 1)
      {
        enemy.idleState.SetFlipAfterIdle(_newPatrolPointIsOnLeft);
      }
      else
      {
        enemy.idleState.SetFlipAfterIdle(!_newPatrolPointIsOnLeft);
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
      MoveTowardsPatrolPoint(_waypoint, stateData.movementSpeed);
    }
  }
}
