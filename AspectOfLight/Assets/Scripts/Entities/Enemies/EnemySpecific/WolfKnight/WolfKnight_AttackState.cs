using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight_AttackState : Enemy_MeleeAttackState
{
    private WolfKnight enemyWolfKnight;

    public WolfKnight_AttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyMeleeAttackState stateData,
        WolfKnight enemy
    )
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemyWolfKnight = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemyWolfKnight.timeSinceAttack = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        float _animationTime = entity.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (_animationTime > 0.40f && !attackedOnce)
        {
            Collider2D[] objectsOverlaped = Physics2D.OverlapCircleAll(
                enemyWolfKnight.attackPosition.transform.position,
                stateData.attackRadius,
                stateData.attackableLayer
            );

            foreach (Collider2D objectHitted in objectsOverlaped)
            {
                // this can change if I want the enemy to attack several entities and not only the player
                objectHitted.gameObject.GetComponent<Player>().TakeDamage(stateData.damage);
            }

            attackedOnce = true;
        }

        if (_animationTime >= 1f)
        {
            stateMachine.ChangeState(enemyWolfKnight.recoveryAttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
