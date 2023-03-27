using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornborne_UGAttackState : State
{
    //TODO when the animation is done update the '_animationTime' to use the GetCurrentAnimatorStateInfo

    private Thornborne enemy;
    private D_ThornborneUGAttackState stateData;

    [Header("State Information")]
    private float timeSincePreviousWave;
    public List<Vector2> referencePoints;

    float _animationTime = 0;

    public Thornborne_UGAttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        Thornborne enemy,
        D_ThornborneUGAttackState stateData
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.rb.velocity = Vector2.zero;

        enemy.canTakeDamage = false;
        timeSincePreviousWave = stateData.ugAttackTickRate;

        _animationTime = 0;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.canTakeDamage = true;
        enemy.timeSinceUGAttack = 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //float _animationTime = entity.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        _animationTime += Time.deltaTime;

        if (_animationTime >= 1) //TODO put the right value to when it's grounded
        {
            timeSincePreviousWave += Time.deltaTime;

            if (timeSincePreviousWave >= stateData.ugAttackTickRate)
            {
                timeSincePreviousWave = 0;

                SpawnNewWave();
            }
        }

        if (_animationTime >= 10)
        {
            stateMachine.ChangeState(enemy.chaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SpawnNewWave()
    {
        List<Vector2> _availableReferencePoints = new List<Vector2>(referencePoints);
        for (int i = 0; i < _availableReferencePoints.Count; i++)
        {
            bool _isReferencePointToClose =
                Vector2.Distance(enemy.transform.position, _availableReferencePoints[i]) <= 3;

            if (_isReferencePointToClose)
                _availableReferencePoints.RemoveAt(i);
        }

        Vector2 _closestReferencePoint = _availableReferencePoints[0];
        for (int i = 1; i < _availableReferencePoints.Count; i++)
        {
            float _currentReferencePointDistance = Vector2.Distance(
                enemy.playerReference.transform.position,
                _availableReferencePoints[i]
            );

            float _closestReferencePointDistance = Vector2.Distance(
                enemy.playerReference.transform.position,
                _closestReferencePoint
            );

            if (_currentReferencePointDistance < _closestReferencePointDistance)
                _closestReferencePoint = _availableReferencePoints[i];
        }

        for (int i = 0; i < stateData.explosionCount; i++)
        {
            int _referencePointIndex = Random.Range(0, _availableReferencePoints.Count - 1);

            entity.CreatePrefab(
                enemy.ugAttackExplosion,
                _availableReferencePoints[_referencePointIndex],
                enemy.abilitiesParent
            );

            _availableReferencePoints.RemoveAt(_referencePointIndex);
        }

        entity.CreatePrefab(
            enemy.ugAttackExplosion,
            enemy.playerReference.transform.position,
            enemy.abilitiesParent
        );

        entity.CreatePrefab(enemy.ugAttackExplosion, _closestReferencePoint, enemy.abilitiesParent);
    }
}
