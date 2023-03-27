using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornborne_ChaseState : Enemy_ChaseState
{
    private Thornborne enemy;
    private D_ThornborneCooldown cooldownData;

    public Thornborne_ChaseState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyChaseState stateData,
        D_ThornborneCooldown cooldownData,
        Thornborne enemy
    )
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
        this.cooldownData = cooldownData;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.target = enemy.playerReference;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        UpdateStates();

        entity.CheckIfFlip(
            enemy.facingDirection,
            enemy.transform.position,
            enemy.target.transform.position
        );
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (enemy.path != null)
        {
            Vector2 _direction =
                enemy.path.vectorPath[enemy.currentWaypoint] - enemy.transform.position;

            entity.rb.velocity = _direction.normalized * stateData.chaseSpeed;
        }
    }

    public void UpdateStates()
    {
        float _distanceToPlayer = Vector2.Distance(
            enemy.transform.position,
            enemy.playerReference.transform.position
        );

        bool _canPukeAttack =
            enemy.timeSincePukeAttack >= cooldownData.pukeAttackCD
            && _distanceToPlayer <= enemy.pukeAttackStateData.pukeAttackRange;

        bool _canUGTeleport =
            enemy.timeSinceUGTeleport >= cooldownData.ugTeleportCD
            && enemy.timeSinceUGAttack <= cooldownData.ugAttackCD
            && _distanceToPlayer >= enemy.ugAttackStateData.ugAttackRange;

        if (_canPukeAttack)
            stateMachine.ChangeState(enemy.pukeAttackState);

        if (_canUGTeleport)
            stateMachine.ChangeState(enemy.uGTeleportState);
    }
}
