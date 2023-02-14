using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_ChaseState : Enemy_ChaseState
{
    private Skeleton enemySkeleton;
    private D_EnemyRangedAttackState attackData;
    private D_EnemyCoolDown coolDownData;

    RaycastHit hit;

    public Skeleton_ChaseState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyChaseState stateData,
        Skeleton enemy,
        D_EnemyRangedAttackState attackData,
        D_EnemyCoolDown coolDownData
    )
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.coolDownData = coolDownData;
        this.attackData = attackData;
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

        entity.CheckIfFlip(
            enemySkeleton.facingDirection,
            enemySkeleton.transform.position,
            enemySkeleton.target.transform.position
        );

        if (
            Vector2.Distance(enemySkeleton.transform.position, enemySkeleton.target.transform.position)
            <= attackData.minimumDistanceToAttack
        )
        {
            if (enemySkeleton.timeSinceAttack >= coolDownData.normalAttackCD)
                stateMachine.ChangeState(enemySkeleton.attackState);
            else
                stateMachine.ChangeState(enemySkeleton.recoveryState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (enemySkeleton.path != null)
        {
            Vector2 _direction =
                enemySkeleton.path.vectorPath[enemySkeleton.currentWaypoint] - enemySkeleton.transform.position;

            entity.rb.velocity = _direction.normalized * stateData.chaseSpeed;
        }
    }
}
