using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornborne_UGTeleportState : State
{
    private D_ThornborneUGTeleportState stateData;
    private Thornborne enemy;

    public bool alreadyTeleported = false;
    public bool teleportToPlayer;
    public bool teleportToSafePoint;

    public Thornborne_UGTeleportState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        Thornborne enemy,
        D_ThornborneUGTeleportState stateData
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.timeSinceUGTeleport = 0;
        enemy.rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();

        alreadyTeleported = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        float _animationTime = entity.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (_animationTime >= 0.5f)
        {
            if (!alreadyTeleported)
            {
                if (teleportToPlayer)
                    TeleportToPlayer();
                else if (teleportToSafePoint)
                    TeleportToSafePoint();

                alreadyTeleported = true;
            }
        }

        if (_animationTime >= 1f)
        {
            if (teleportToPlayer)
                stateMachine.ChangeState(enemy.chaseState);

            if (teleportToSafePoint)
            {
                if (
                    enemy.currentHealth <= stateData.minimumLifeToHeal
                    && enemy.timeSinceHealing >= enemy.cooldownData.healingCD
                )
                    stateMachine.ChangeState(enemy.healingState);
                else
                    stateMachine.ChangeState(enemy.ugAttackState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void TeleportToPlayer()
    {
        alreadyTeleported = true;

        Transform _playerTransform = enemy.playerReference.transform;

        int _playerFacingDirection = enemy.playerReference.GetComponent<Entity>().facingDirection;

        Vector3 _newPosition =
            _playerTransform.position
            + new Vector3(
                (_playerTransform.localScale.x + 1.5f) * (_playerFacingDirection * -1),
                0f,
                0f
            );

        Collider2D[] _collidersInNewPosition = Physics2D.OverlapCircleAll(
            _newPosition,
            0.5f,
            LayerMask.GetMask("Wall", "Water", "TopGround")
        );

        if (_collidersInNewPosition.Length != 0)
        {
            _newPosition.x = _newPosition.x * -1;
        }

        enemy.rb.MovePosition(_newPosition);
    }

    private void TeleportToSafePoint()
    {
        Vector2 currentSafePoint = FindNewSafePoint();
        enemy.rb.MovePosition(currentSafePoint);
    }

    private Vector2 FindNewSafePoint()
    {
        GameObject[] safePoints = enemy.safePoints;
        GameObject _farthestReferencePoint = safePoints[0];

        for (int i = 1; i < safePoints.Length; i++)
        {
            float _currentReferencePointDistance = Vector2.Distance(
                enemy.playerReference.transform.position,
                safePoints[i].transform.position
            );

            float _farthestReferencePointDistance = Vector2.Distance(
                enemy.playerReference.transform.position,
                _farthestReferencePoint.transform.position
            );

            if (_currentReferencePointDistance > _farthestReferencePointDistance)
                _farthestReferencePoint = safePoints[i];
        }

        return _farthestReferencePoint.transform.position;
    }
}
