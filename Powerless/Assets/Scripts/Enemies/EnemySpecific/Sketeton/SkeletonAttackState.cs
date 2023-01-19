using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyShootingState
{

  private Skeleton skeletonEnemy;
  private D_EnemyData enemyData;
  private D_EnemyShootingAttack stateData;
  private D_EnemyCoolDownData coolDownData;

  public SkeletonAttackState(
      Entity entity,
      StateMachine stateMachine,
      string animBoolName,
      D_EnemyShootingAttack stateData,
      Skeleton enemy
  ) : base(entity, stateMachine, animBoolName)
  {
    this.stateData = stateData;
    this.skeletonEnemy = enemy;
  }

  public override void Enter()
  {
    base.Enter();
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
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();
  }

  public override void UpdateCooldowns()
  {
    base.UpdateCooldowns();
  }
}
