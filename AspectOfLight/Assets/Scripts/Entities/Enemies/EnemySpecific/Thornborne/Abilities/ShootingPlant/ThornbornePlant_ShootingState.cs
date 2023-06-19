using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornbornePlant_ShootingState : State
{
    ThornbornePlant enemy;
    float animationTime;

    public ThornbornePlant_ShootingState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        ThornbornePlant enemy
    )
        : base(entity, stateMachine, animBoolName)
    {
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

        animationTime = enemy.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (animationTime >= 1)
        {
            GameObject _bullet = entity.CreatePrefab(
                enemy.bullet,
                enemy.shootingPosition.position,
                enemy.bulletParent
            );
            _bullet.GetComponent<Bullet>().targetPosition = enemy.plantTarget.transform.position;

            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
