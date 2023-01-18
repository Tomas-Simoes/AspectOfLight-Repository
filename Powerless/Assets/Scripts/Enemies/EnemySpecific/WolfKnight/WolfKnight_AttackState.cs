using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight_AttackState : EnemyMeleeAttack
{
    private WolfKnight enemy;
    private D_EnemyMeleeAttackData stateData;

    public WolfKnight_AttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyMeleeAttackData stateData,
        WolfKnight enemy
    ) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.timeSinceAttack = 0;
        attackedOnce = false;
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
                enemy.attackPosition.transform.position,
                stateData.attackRadius,
                stateData.attackableLayer
            );

            foreach (Collider2D objectHitted in objectsOverlaped)
            {
                Debug.Log("This got hitted" + objectHitted);
            }

            attackedOnce = true;
        }

        if (_animationTime > 1f)
        {
            stateMachine.ChangeState(enemy.recoveryAttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
