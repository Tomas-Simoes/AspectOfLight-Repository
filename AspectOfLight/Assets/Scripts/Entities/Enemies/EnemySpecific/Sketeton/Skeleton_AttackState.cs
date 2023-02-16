using UnityEngine;

public class Skeleton_AttackState : Enemy_RangedAttackState
{
    private Skeleton enemySkeleton;

    public Skeleton_AttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyRangedAttackState stateData,
        Skeleton enemy
    )
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemySkeleton = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemySkeleton.timeSinceAttack = 0;

        GameObject _bullet = entity.CreatePrefab(
            enemySkeleton.bulletPrefab,
            enemySkeleton.attackPosition.transform.position,
            enemySkeleton.bulletsParent
        );

        _bullet.GetComponent<Bullet>().bulletParent = enemySkeleton;
        stateMachine.ChangeState(enemySkeleton.recoveryState);
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
