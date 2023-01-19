using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonChaseState : EnemyChaseState
{
  private Skeleton enemySkeleton;
  private D_EnemyData enemyData;
  private D_EnemyShootingAttack attackData;
  private D_EnemyCoolDownData coolDownData;

  public SkeletonChaseState(
      Entity entity,
      StateMachine stateMachine,
      string animBoolName,
      D_EnemyChaseState stateData,
      Skeleton enemy,
      D_EnemyData enemyData,
      D_EnemyShootingAttack attackData,
      D_EnemyCoolDownData coolDownData
  ) : base(entity, stateMachine, animBoolName, stateData)
  {
    this.coolDownData = coolDownData;
    this.attackData = attackData;
    this.enemyData = enemyData;
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

  public override void GetInputs()
  {
    base.GetInputs();
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();
     if (
            Vector2.Distance(enemySkeleton.transform.position, enemySkeleton.target.transform.position)
                <= attackData.minimumDistanceToAttack
            && coolDownData.timeSinceNormalAttack >= coolDownData.normalAttackCD
        )
        {
            stateMachine.ChangeState(enemySkeleton.attackState);
        }

        entity.CheckIfFlip(
            enemySkeleton.facingDirection,
            enemySkeleton.transform.position,
            enemySkeleton.target.transform.position
        );

        coolDownData.timeSinceNormalAttack += Time.deltaTime;
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();
    StartChasing(enemySkeleton.target);
  }

  public override void UpdateCooldowns()
  {
    base.UpdateCooldowns();
  }
}
