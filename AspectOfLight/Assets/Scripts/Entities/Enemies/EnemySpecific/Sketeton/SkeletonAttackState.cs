using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyRangedAttackState
{
    private Skeleton enemy;
    private D_EnemyCoolDown coolDownData;

    public SkeletonAttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyRangedAttackState stateData,
        Skeleton enemy
    )
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.timeSinceAttack = 0;

        GameObject _bullet = entity.CreatePrefab(
            enemy.bulletPrefab,
            enemy.attackPosition.transform.position,
            enemy.bulletsParent
        );

        _bullet.GetComponent<Bullet>().bulletParent = enemy;
        stateMachine.ChangeState(enemy.recoveryState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
