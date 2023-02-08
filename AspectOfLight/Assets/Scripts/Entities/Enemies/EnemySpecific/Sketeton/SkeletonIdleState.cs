using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonidleState : EnemyIdleState
{
  private Skeleton enemySkeleton;

  public SkeletonidleState(
      Entity entity,
      StateMachine stateMachine,
      string animBoolName,
      D_EnemyIdleState stateData,
      Skeleton enemy
  ) : base(entity, stateMachine, animBoolName, stateData)
  {
    this.enemySkeleton = enemy;
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

    if (isIdleTimeOver)
    {
      stateMachine.ChangeState(enemySkeleton.moveState);
    }
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();
  }
}
