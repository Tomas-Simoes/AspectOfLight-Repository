using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight_RecoveryAttackState : EnemyRecoveryAttackState
{
    private WolfKnight enemy;
    private D_EnemyMeleeAttackData attackData;
    private D_EnemyCoolDownData coolDownData;
    private D_EnemyMoveState moveStateData;

    private bool goTowardsTarget;
    private bool goTowardsRecoveryPoint;
    private Collider2D _collider;
    public Vector2 recoveryPoint;

    public WolfKnight_RecoveryAttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyRecoveryAttackState stateData,
        WolfKnight enemy,
        D_EnemyMeleeAttackData attackData,
        D_EnemyCoolDownData coolDownData,
        D_EnemyMoveState moveStateData
    )
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
        this.attackData = attackData;
        this.coolDownData = coolDownData;
        this.moveStateData = moveStateData;
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.timeSinceAttack >= coolDownData.normalAttackCD)
            stateMachine.ChangeState(enemy.chaseState);

        if (
            Vector2.Distance(enemy.target.transform.position, enemy.pivot.transform.position)
            > stateData.recoveryAreaRadius + 0.5f
        )
        {
            goTowardsTarget = true;
            goTowardsRecoveryPoint = false;
        }
        else
        {
            bool _createNewRecoveryPoint =
                !goTowardsRecoveryPoint
                || Vector2.Distance((Vector2)recoveryPoint, enemy.pivot.transform.position) < 0.5f;

            if (_createNewRecoveryPoint)
            {
                do
                {
                    totalIterationsMade++;

                    if (totalIterationsMade >= MAX_ITERATIONS)
                    {
                        stateMachine.ChangeState(enemy.moveState);
                    }

                    recoveryPoint = GetNewRandomPosition(enemy.target.transform.position);

                    _collider = Physics2D
                        .Linecast(
                            entity.pivot.transform.position,
                            recoveryPoint,
                            entity.entityData.whatIsWall
                        )
                        .collider;
                } while (_collider != null);

                goTowardsRecoveryPoint = true;
            }

            goTowardsTarget = false;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (goTowardsTarget)
        {
            entity.CheckIfFlip(
                entity.facingDirection,
                enemy.transform.position,
                enemy.target.transform.position
            );

            entity.GoTowardsTarget(enemy.target.transform.position, moveStateData.movementSpeed);
        }

        if (goTowardsRecoveryPoint)
        {
            entity.CheckIfFlip(
                entity.facingDirection,
                enemy.transform.position,
                (Vector2)recoveryPoint
            );

            entity.GoTowardsTarget((Vector2)recoveryPoint, stateData.recoveryMovementSpeed);
        }
    }
}
