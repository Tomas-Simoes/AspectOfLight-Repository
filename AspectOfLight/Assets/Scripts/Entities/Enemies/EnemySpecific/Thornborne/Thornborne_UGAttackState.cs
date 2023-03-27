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
    private int currentWavesCount;
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
        currentWavesCount = 0;
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

        if (currentWavesCount >= stateData.wavesCount)
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
                Vector2.Distance(enemy.transform.position, _availableReferencePoints[i]) <= 3.5;

            if (_isReferencePointToClose)
                _availableReferencePoints.RemoveAt(i);
        }

        Vector2 _closestReferencePoint = GetClosestReferencePointToPlayer(
            _availableReferencePoints
        );

        entity.CreatePrefab(
            enemy.ugAttackExplosion,
            enemy.playerReference.transform.position,
            enemy.abilitiesParent
        );

        entity.CreatePrefab(enemy.ugAttackExplosion, _closestReferencePoint, enemy.abilitiesParent);

        SpawnExplosions(_availableReferencePoints);

        currentWavesCount++;
    }

    private Vector2 GetClosestReferencePointToPlayer(List<Vector2> referencePoints)
    {
        Vector2 _closestReferencePoint = referencePoints[0];

        for (int i = 1; i < referencePoints.Count; i++)
        {
            float _currentReferencePointDistance = Vector2.Distance(
                enemy.playerReference.transform.position,
                referencePoints[i]
            );

            float _closestReferencePointDistance = Vector2.Distance(
                enemy.playerReference.transform.position,
                _closestReferencePoint
            );

            if (_currentReferencePointDistance < _closestReferencePointDistance)
                _closestReferencePoint = referencePoints[i];
        }

        return _closestReferencePoint;
    }

    private void SpawnExplosions(List<Vector2> referencePoints)
    {
        int _plantsCount = 0;

        for (int i = 0; i < stateData.explosionCount; i++)
        {
            int _referencePointIndex = Random.Range(0, referencePoints.Count - 1);

            entity.CreatePrefab(
                enemy.ugAttackExplosion,
                referencePoints[_referencePointIndex],
                enemy.abilitiesParent
            );

            if (
                currentWavesCount >= stateData.wavesCount - 1
                && _plantsCount <= stateData.plantsCount
            )
            {
                GameObject _shootingPlant = entity.CreatePrefab(
                    enemy.shootingPlant,
                    referencePoints[_referencePointIndex],
                    enemy.abilitiesParent
                );

                ThornborneShootingPlant _shootingPlantComponent =
                    _shootingPlant.GetComponent<ThornborneShootingPlant>();

                _shootingPlantComponent.bulletParent = enemy.abilitiesParent.transform;
                _shootingPlantComponent.plantTarget = enemy.target;

                _plantsCount++;
            }

            referencePoints.RemoveAt(_referencePointIndex);
        }
    }
}
