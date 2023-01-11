using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : State
{
  protected D_EnemyMoveState stateData;
  protected bool isDetectingWall;

  public int currentPatrolIndex = 0;

  public EnemyMoveState(
      Entity entity,
      StateMachine stateMachine,
      string animBoolName,
      D_EnemyMoveState stateData
  ) : base(entity, stateMachine, animBoolName)
  {
    this.stateData = stateData;
  }

  public override void Enter()
  {
    base.Enter();
    entity.SetVelocity(stateData.movementSpeed);

    isDetectingWall = entity.CheckWall();
  }

  public override void Exit()
  {
    base.Exit();
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();
  }

  public void SetCurrentPatrolIndex(int newIndex)
  {
    currentPatrolIndex = newIndex;
  }

  public void MoveTowardsPatrolPoint(GameObject patrolPoint, float movementSpeed)
  {
    entity.rb.MovePosition(
        Vector3.MoveTowards(
            entity.transform.position,
            patrolPoint.transform.position,
            movementSpeed * Time.fixedDeltaTime
        )
    );
  }
}
