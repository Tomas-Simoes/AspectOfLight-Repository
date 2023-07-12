using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThePlagueChaseState : State
{
    private float timeSinceClosestEnemyCheck = 0;

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
        timeSinceClosestEnemyCheck += Time.deltaTime;

        if (plague.closestEnemy == null)
        {
            if (timeSinceClosestEnemyCheck >= stateData.minTimeToCheckClosestEnemy)
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

        Vector2 _direction;

        if (plague.closestEnemy == null)
        {
            float _distanceToPlayer = Vector2.Distance(
                plague.transform.position,
                GameManager.Instance.playerReference.transform.position
            );

            if (_distanceToPlayer >= stateData.minDistanceToPlayer)
                _direction =
                    GameManager.Instance.playerReference.transform.position
                    - plague.transform.position;
            else
                _direction = new Vector2(0, 0);
        }
        else
            _direction = plague.closestEnemy.transform.position - plague.transform.position;

        plague.rb.velocity = _direction.normalized * stateData.movementSpeed;
    }

    private void GetClosestEnemy()
    {
        Debug.Log("finding new enemy");
        plague.closestEnemy = null;

        timeSinceClosestEnemyCheck = 0;
        GameObject[] _allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        float _largerDistance = 0;

        foreach (GameObject enemy in _allEnemies)
        {
            float _currentDistance = Vector2.Distance(
                enemy.transform.position,
                plague.transform.position
            );

            Debug.Log(_currentDistance);
            Debug.Log(_largerDistance);
            Debug.Log(stateData.maxDistanceToChase);

            if (_currentDistance >= _largerDistance)
            {
                if (!(_currentDistance >= stateData.maxDistanceToChase))
                {
                    _largerDistance = _currentDistance;
                    plague.closestEnemy = enemy;
                }
            }
        }

        Debug.Log(plague.closestEnemy);
    }

    private void ChasePlayer() { }
}
