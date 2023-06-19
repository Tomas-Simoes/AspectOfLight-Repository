using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThePlagueChaseState : State
{
    private ThePlague plague;
    protected D_PlagueChaseState stateData;
    private D_PlagueAttackState attackStateData;

    public ThePlagueChaseState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_PlagueChaseState stateData,
        ThePlague plague,
        D_PlagueAttackState attackStateData
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.attackStateData = attackStateData;
        this.plague = plague;
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        GetClosestEnemy();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (plague.closestEnemy == null)
        {
            GetClosestEnemy();
            return;
        }

        if (
            Vector3.Distance(plague.transform.position, plague.closestEnemy.transform.position)
            <= attackStateData.minimumDistanceToAttack
        )
        {
            stateMachine.ChangeState(plague.attackState);
        }

        entity.CheckIfFlip(
            entity.facingDirection,
            entity.transform.position,
            plague.closestEnemy.transform.position
        );
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (plague.closestEnemy == null)
            return;

        entity.GoTowardsPosition(plague.closestEnemy.transform.position, stateData.movementSpeed);
    }

    private void GetClosestEnemy()
    {
        plague.closestEnemy = null;

        GameObject[] _allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        float _distance = Mathf.Infinity;

        foreach (GameObject enemy in _allEnemies)
        {
            Vector3 diff = enemy.transform.position - entity.transform.position;
            float _currentDistance = diff.sqrMagnitude;

            if (_currentDistance < _distance)
            {
                plague.closestEnemy = enemy;
                _distance = _currentDistance;
            }
        }
    }
}
