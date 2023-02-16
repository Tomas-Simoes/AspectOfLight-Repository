using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfTank_ChaseState : Enemy_ChaseState
{
    private WolfTank enemyWolfTank;
    private D_EnemyRangedAttackState attackData;
    private D_EnemyCoolDown coolDownData;

    public WolfTank_ChaseState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyChaseState stateData,
        WolfTank enemy,
        D_EnemyRangedAttackState attackData,
        D_EnemyCoolDown coolDownData
    )
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemyWolfTank = enemy;
        this.coolDownData = coolDownData;
        this.attackData = attackData;
    }

    public override void Enter()
    {
        base.Enter();
        enemyWolfTank.target = enemyWolfTank.playerReference;
        enemyWolfTank.timeSinceAttack = coolDownData.normalAttackCD;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        entity.CheckIfFlip(
            enemyWolfTank.facingDirection,
            enemyWolfTank.transform.position,
            enemyWolfTank.target.transform.position
        );

        if (
            Vector2.Distance(enemyWolfTank.transform.position, enemyWolfTank.target.transform.position)
            <= attackData.minimumDistanceToAttack
        )
        {
            if (enemyWolfTank.timeSinceAttack >= coolDownData.normalAttackCD)
                stateMachine.ChangeState(enemyWolfTank.attackState);
            else
                stateMachine.ChangeState(enemyWolfTank.recoveryAttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (enemyWolfTank.path != null)
        {
            Vector2 _direction =
                enemyWolfTank.path.vectorPath[enemyWolfTank.currentWaypoint] - enemyWolfTank.transform.position;

            entity.rb.velocity = _direction.normalized * stateData.chaseSpeed;
        }
    }
}
