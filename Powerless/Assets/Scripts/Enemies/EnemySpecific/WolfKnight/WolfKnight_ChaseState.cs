using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight_ChaseState : EnemyChaseState
{
    private WolfKnight enemy;
    private D_EnemyMeleeAttackData attackData;
    private D_EnemyCoolDownData coolDownData;

    public WolfKnight_ChaseState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyChaseState stateData,
        WolfKnight enemy,
        D_EnemyMeleeAttackData attackData,
        D_EnemyCoolDownData coolDownData
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

        enemy.timeSinceAttack = coolDownData.normalAttackCD;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (
            Vector2.Distance(enemy.transform.position, enemy.target.transform.position)
            <= attackData.minimumDistanceToAttack
        )
        {
            if (enemy.timeSinceAttack >= coolDownData.normalAttackCD)
                stateMachine.ChangeState(enemy.attackState);
        }

        entity.CheckIfFlip(
            enemy.facingDirection,
            enemy.transform.position,
            enemy.target.transform.position
        );
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        entity.GoTowardsTarget(enemy.target.transform.position, stateData.chaseSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemy.transform.position, attackData.minimumDistanceToAttack);
    }
}
