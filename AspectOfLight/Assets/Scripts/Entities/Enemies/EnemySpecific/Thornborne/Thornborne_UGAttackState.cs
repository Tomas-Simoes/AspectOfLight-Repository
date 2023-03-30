using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Thornborne_UGAttackState : State
{
    //TODO when the animation is done update the '_animationTime' to use the GetCurrentAnimatorStateInfo

    private Thornborne enemy;
    private D_ThornborneUGAttackState stateData;

    [Header("State Information")]
    public bool canSpawnWaves;
    private float timeSincePreviousWave;
    public List<Vector2> referencePoints;
    private int wavesCount;
    private int currentWavesCount;
    private Dictionary<Vector2, GameObject> currentShootingPlants =
        new Dictionary<Vector2, GameObject>();

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

        UpdateCurrentShootingPlants();

        enemy.anim.SetBool("ugAttackUp", false);

        enemy.rb.velocity = Vector2.zero;

        canSpawnWaves = false;
        enemy.canTakeDamage = false;
        timeSincePreviousWave = stateData.ugAttackTickRate;

        wavesCount = Random.Range(stateData.minWavesCount, stateData.maxWavesCount);

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

        if (canSpawnWaves && currentWavesCount < wavesCount)
        {
            timeSincePreviousWave += Time.deltaTime;

            if (timeSincePreviousWave >= stateData.ugAttackTickRate)
            {
                timeSincePreviousWave = 0;
        
                SpawnNewWave();
            }
        }

        if (currentWavesCount >= wavesCount)
        {

            enemy.anim.SetBool("ugAttackUp", true);

            float _animationTime = entity.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (!entity.anim.GetCurrentAnimatorStateInfo(0).IsName("UGAttack_Up"))
                return;

            if (_animationTime >= 1)
            {
                stateMachine.ChangeState(enemy.chaseState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void UpdateCurrentShootingPlants()
    {
        for (int i = currentShootingPlants.Count - 1; i >= 0; i--)
        {
            KeyValuePair<Vector2, GameObject> _currentShootingPlant =
                currentShootingPlants.ElementAt(i);

            if (_currentShootingPlant.Value == null)
                currentShootingPlants.Remove(_currentShootingPlant.Key);
        }
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

        entity.CreatePrefab(enemy.ugAttackExplosion, _closestReferencePoint, enemy.abilitiesParent);
        entity.CreatePrefab(
            enemy.ugAttackExplosion,
            enemy.playerReference.transform.position,
            enemy.abilitiesParent);
            
        if (currentWavesCount >= wavesCount - 1)
            UpgradePreviousPlants();

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
        int _plantsCount = currentShootingPlants.Count;

        for (int i = 0; i < stateData.explosionCount; i++)
        {
            int _referencePointIndex = Random.Range(0, referencePoints.Count - 1);

            entity.CreatePrefab(
                enemy.ugAttackExplosion,
                referencePoints[_referencePointIndex],
                enemy.abilitiesParent
            );

            if (currentWavesCount >= wavesCount - 1 && _plantsCount <= stateData.plantsCount)
            {
                if (!currentShootingPlants.ContainsKey(referencePoints[_referencePointIndex]))
                {
                    GameObject _shootingPlant = entity.CreatePrefab(
                        enemy.shootingPlant,
                        referencePoints[_referencePointIndex],
                        enemy.abilitiesParent
                    );

                    currentShootingPlants.Add(
                        referencePoints[_referencePointIndex],
                        _shootingPlant
                    );

                    ThornbornePlant _shootingPlantComponent =
                        _shootingPlant.GetComponent<ThornbornePlant>();

                    _shootingPlantComponent.bulletParent = enemy.abilitiesParent;
                    _shootingPlantComponent.plantTarget = enemy.target;

                    _plantsCount++;
                }
            }

            referencePoints.RemoveAt(_referencePointIndex);
        }
    }

    private void UpgradePreviousPlants()
    {
        foreach (GameObject shootingPlant in currentShootingPlants.Values)
        {
            if (shootingPlant == null)
                break;

            shootingPlant.GetComponent<ThornbornePlant>().shootingRate /= 2;
        }
    }
}
