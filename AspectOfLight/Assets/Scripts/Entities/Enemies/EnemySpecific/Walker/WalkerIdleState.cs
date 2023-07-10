using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerIdleState : Enemy_IdleState
{
      private Walker enemy;

  public WalkerIdleState(
      Entity entity,
      StateMachine stateMachine,
      string animBoolName,
      D_EnemyIdleState stateData,
      Walker enemy
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

            entity.CheckIfFlip(
            enemy.facingDirection,
            enemy.transform.position,
            enemy.target.transform.position
        );
    
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();

    if (isIdleTimeOver)
    {
      stateMachine.ChangeState(enemy.patrolState);
    }
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();
  }
}
