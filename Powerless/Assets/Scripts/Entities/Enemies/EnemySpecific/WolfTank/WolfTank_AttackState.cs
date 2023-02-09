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
        Debug.Log("Attack");
        enemy.timeSinceAttack = 0;

        GameObject _bullet = entity.CreatePrefab(
            enemy.bulletPrefab,
            enemy.attackPosition.transform.position,
            enemy.bulletsParent
        );

        _bullet.GetComponent<bulletForEnemy>().bulletParent = enemy;

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
