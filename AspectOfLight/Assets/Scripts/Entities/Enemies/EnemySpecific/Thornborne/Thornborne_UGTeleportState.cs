using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornborne_UGTeleportState : State
{
    //TODO change this to animation time
    private D_ThornborneUGTeleportState stateData;
    private Thornborne enemy;
    private float animationTime;

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

        animationTime += Time.deltaTime;

        if (animationTime >= 1)
        {
            if (teleportToPlayer)
                TeleportToPlayer();
            else if (teleportToSafePoint)
                TeleportToSafePoint();
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

        stateMachine.ChangeState(enemy.chaseState); //TODO change this to melee attack state
    }

    private void TeleportToSafePoint()
    {
        Vector2 currentSafePoint = FindNewSafePoint();
        enemy.rb.MovePosition(currentSafePoint);

        if (enemy.currentHealth <= stateData.minimumLifeToHeal)
            stateMachine.ChangeState(enemy.healingState);
        else
            stateMachine.ChangeState(enemy.ugAttackState);
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
