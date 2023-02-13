using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfTank_AttackState : EnemyRangedAttackState
{
    private WolfTank enemy;

    public WolfTank_AttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyRangedAttackState stateData,
        WolfTank enemy
    )
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        //float _animationTime = entity.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        enemy.timeSinceAttack = 0;

        //if(_animationTime >= 1f){
        GameObject _bullet = entity.CreatePrefab(
            enemy.bulletPrefab,
            enemy.attackPosition.transform.position,
            enemy.bulletsParent
        );

        _bullet.GetComponent<Bullet>().bulletParent = enemy;
        stateMachine.ChangeState(enemy.recoveryAttackState);
        //}


        //stateMachine.ChangeState(enemy.recoveryState);
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
