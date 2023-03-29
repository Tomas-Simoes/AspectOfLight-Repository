using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornborne_UGTeleportState : State
{
    private D_ThornborneUGTeleportState stateData;
    private Thornborne enemy;

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
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        float _animationTime = entity.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (_animationTime >= 0.5f)
        {
            if (teleportToPlayer)
                TeleportToPlayer();
            else if (teleportToSafePoint)
                TeleportToSafePoint();
        }

        if (_animationTime >= 1f)
        {
            if (teleportToPlayer)
                stateMachine.ChangeState(enemy.chaseState);

            if (teleportToSafePoint)
            {
                if (enemy.currentHealth <= stateData.minimumLifeToHeal)
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
        Transform _playerTransform = enemy.playerReference.transform;

        Vector3 _newPosition =
            _playerTransform.position
            + new Vector3(_playerTransform.localScale.x + 1f, 0f, 0f)
                * enemy.playerReference.GetComponent<Entity>().facingDirection;

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
