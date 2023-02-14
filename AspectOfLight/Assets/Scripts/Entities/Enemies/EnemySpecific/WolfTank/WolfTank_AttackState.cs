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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float _animationTime = entity.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (_animationTime >= 0.77f && !attackedOnce)
        {
            GameObject _bullet = entity.CreatePrefab(
                enemy.bulletPrefab,
                enemy.attackPosition.transform.position,
                enemy.bulletsParent
            );

            _bullet.GetComponent<Bullet>().bulletParent = enemy;

            attackedOnce = true;
        }

        if (_animationTime >= 1f)
        {
            enemy.timeSinceAttack = 0;
            stateMachine.ChangeState(enemy.recoveryAttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
