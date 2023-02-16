using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight_ChaseState : Enemy_ChaseState
{
    private WolfKnight enemyWolfKnight;
    private D_EnemyMeleeAttackState attackData;
    private D_EnemyCoolDown coolDownData;

    public WolfKnight_ChaseState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyChaseState stateData,
        WolfKnight enemy,
        D_EnemyMeleeAttackState attackData,
        D_EnemyCoolDown coolDownData
    )
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemyWolfKnight = enemy;
        this.coolDownData = coolDownData;
        this.attackData = attackData;
    }

    public override void Enter()
    {
        base.Enter();
        enemyWolfKnight.target = enemyWolfKnight.playerReference;
        enemyWolfKnight.timeSinceAttack = coolDownData.normalAttackCD;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        entity.CheckIfFlip(
            enemyWolfKnight.facingDirection,
            enemyWolfKnight.transform.position,
            enemyWolfKnight.target.transform.position
        );

        if (
            Vector2.Distance(enemyWolfKnight.transform.position, enemyWolfKnight.target.transform.position)
            <= attackData.minimumDistanceToAttack
        )
        {
            if (enemyWolfKnight.timeSinceAttack >= coolDownData.normalAttackCD)
                stateMachine.ChangeState(enemyWolfKnight.attackState);
            else
                stateMachine.ChangeState(enemyWolfKnight.recoveryAttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (enemyWolfKnight.path != null)
        {
            Vector2 _direction =
                enemyWolfKnight.path.vectorPath[enemyWolfKnight.currentWaypoint] - enemyWolfKnight.transform.position;

            entity.rb.velocity = _direction.normalized * stateData.chaseSpeed;
        }
    }
}
