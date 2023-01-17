using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight_RecoveryAttackState : EnemyRecoveryAttackState
{
    private WolfKnight enemy;
    private D_EnemyMeleeAttackData attackData;
    private D_EnemyCoolDownData coolDownData;

    public WolfKnight_RecoveryAttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyRecoveryAttackState stateData,
        WolfKnight enemy,
        D_EnemyMeleeAttackData attackData,
        D_EnemyCoolDownData coolDownData
    ) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
        this.attackData = attackData;
        this.coolDownData = coolDownData;
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
        bool _enemyInDistanceToAttack =
            Vector2.Distance(enemy.transform.position, enemy.target.transform.position)
            <= attackData.minimumDistanceToAttack;

        if (!_enemyInDistanceToAttack)
        {
            stateMachine.ChangeState(enemy.chaseState);
        }
        else
        {
            entity.SetVelocity(0);
            Debug.Log("Recovering");
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
