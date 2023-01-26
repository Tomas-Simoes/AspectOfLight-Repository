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
        /*
        GameObject _bulletInstance = entity.InstantiateObject(
            stateData.bullet,
            enemy.attackPosition.transform.position,
            Quaternion.identity
        );

        _bulletInstance.transform.parent = GameObject.Find("Enemies Bullets").transform;

        stateMachine.ChangeState(enemy.chaseState);
        */
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
