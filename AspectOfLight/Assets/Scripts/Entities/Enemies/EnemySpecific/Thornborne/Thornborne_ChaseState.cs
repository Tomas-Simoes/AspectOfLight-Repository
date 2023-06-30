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

        if (
            enemy.path != null
            && Vector2.Distance(enemy.transform.position, enemy.playerReference.transform.position)
                >= stateData.minDistanceBetweenPlayer
        )
        {
            Vector2 _direction =
                enemy.path.vectorPath[enemy.currentWaypoint] - enemy.transform.position;

            entity.rb.velocity = _direction.normalized * stateData.chaseSpeed;
        }
    }

    public void UpdateStates()
    {
        if (stateMachine.currentState == enemy.healingState)
            return;

        float _distanceToPlayer = Vector2.Distance(
            enemy.transform.position,
            enemy.playerReference.transform.position
        );

        bool _canPukeAttack =
            enemy.timeSincePukeAttack >= cooldownData.pukeAttackCD
            && _distanceToPlayer <= enemy.pukeAttackStateData.pukeAttackRange;

        bool _canUGTeleport = enemy.timeSinceUGTeleport >= cooldownData.ugTeleportCD;

        bool _canUGAttack = enemy.timeSinceUGAttack >= cooldownData.ugAttackCD;

        //? if the enemy is low life, it will teleport to the safe point
        if (enemy.currentHealth <= enemy.ugTeleportStateData.minimumLifeToHeal && _canUGTeleport)
        {
            enemy.uGTeleportState.teleportToPlayer = false;
            enemy.uGTeleportState.teleportToSafePoint = true;

            stateMachine.ChangeState(enemy.uGTeleportState);
        }

        //? if the enemy has cooldown for UG Attack but it's close range, it will teleport to the safe point
        if (
            _canUGAttack
            && _canUGTeleport
            && _distanceToPlayer <= enemy.ugAttackStateData.ugAttackRange
        )
        {
            enemy.uGTeleportState.teleportToPlayer = false;
            enemy.uGTeleportState.teleportToSafePoint = true;

            stateMachine.ChangeState(enemy.uGTeleportState);
        }

        //? if the enemy can't UG Attack but can UG Teleport and it's long range, it will teleport towards the player
        if (
            !_canUGAttack
            && _canUGTeleport
            && _distanceToPlayer >= enemy.ugAttackStateData.ugAttackRange
        )
        {
            enemy.uGTeleportState.teleportToPlayer = true;
            enemy.uGTeleportState.teleportToSafePoint = false;

            stateMachine.ChangeState(enemy.uGTeleportState);
        }
        //? if the player has no cooldown for UG Attack or UG Teleport and it's close range or it's tooo close to the player, it will puke
        if (_canPukeAttack || _distanceToPlayer <= stateData.minDistanceBetweenPlayer)
            stateMachine.ChangeState(enemy.pukeAttackState);
    }
}
