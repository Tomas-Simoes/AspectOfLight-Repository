using UnityEngine;

public class WolfTank_AttackState : Enemy_RangedAttackState
{
    private WolfTank enemyWolfTank;

    public WolfTank_AttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyRangedAttackState stateData,
        WolfTank enemy
    )
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemyWolfTank = enemy;
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
                enemyWolfTank.bulletPrefab,
                enemyWolfTank.attackPosition.transform.position,
                enemyWolfTank.bulletsParent
            );

            _bullet.GetComponent<Bullet>().bulletParent = enemyWolfTank;

            attackedOnce = true;
        }

        if (_animationTime >= 1f)
        {
            enemyWolfTank.timeSinceAttack = 0;
            stateMachine.ChangeState(enemyWolfTank.recoveryAttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
