using UnityEngine;

public class Skeleton_AttackState : Enemy_RangedAttackState
{
    private Skeleton enemy;

    public Skeleton_AttackState(
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

        _bullet.GetComponent<Bullet>().targetPosition = enemy.target.transform.position;
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
