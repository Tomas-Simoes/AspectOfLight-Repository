using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_ChaseState : Enemy_ChaseState
{
    private Skeleton enemy;
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

        entity.CheckIfFlip(
            enemy.facingDirection,
            enemy.transform.position,
            enemy.target.transform.position
        );

        if (
            Vector2.Distance(enemy.transform.position, enemy.target.transform.position)
            <= attackData.minimumDistanceToAttack
        )
        {
            if (enemy.timeSinceAttack >= coolDownData.normalAttackCD)
                stateMachine.ChangeState(enemy.attackState);
            else
                stateMachine.ChangeState(enemy.recoveryState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (enemy.path != null)
        {
            Vector2 _direction =
                enemy.path.vectorPath[enemy.currentWaypoint] - enemy.transform.position;

            entity.rb.velocity = _direction.normalized * stateData.chaseSpeed;
        }
    }
}
