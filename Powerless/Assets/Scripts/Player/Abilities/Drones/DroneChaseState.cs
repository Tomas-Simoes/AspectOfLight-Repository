using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneChaseState : State
{
    protected bool isDetectingWall;
    private Drone drone;

    protected D_DroneChaseState stateData;
    private D_DroneAttackState attackStateData;

    public DroneChaseState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_DroneChaseState stateData,
        Drone drone,
        D_DroneAttackState attackStateData
    ) : base(entity, stateMachine, animBoolName)
    {
        this.attackStateData = attackStateData;
        this.drone = drone;
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        drone.closestEnemy = null;

        GameObject[] _allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        float _distance = Mathf.Infinity;

        foreach (GameObject enemy in _allEnemies)
        {
            Vector3 diff = enemy.transform.position - entity.transform.position;
            float _currentDistance = diff.sqrMagnitude;

            if (_currentDistance < _distance)
            {
                drone.closestEnemy = enemy;
                _distance = _currentDistance;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void GetInputs()
    {
        base.GetInputs();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (
            Vector3.Distance(drone.transform.position, drone.closestEnemy.transform.position)
            <= attackStateData.minimumDistanceToAttack
        )
        {
            stateMachine.ChangeState(drone.attackState);
        }

        entity.CheckIfFlip(
            entity.facingDirection,
            entity.transform.position,
            drone.closestEnemy.transform.position
        );
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        StartChasing(drone.closestEnemy);
    }

    public void StartChasing(GameObject closestEnemy)
    {
        entity.rb.MovePosition(
            Vector3.MoveTowards(
                entity.transform.position,
                closestEnemy.transform.position,
                stateData.droneChaseSpeed * Time.fixedDeltaTime
            )
        );
    }

    public override void UpdateCooldowns()
    {
        base.UpdateCooldowns();
    }
}
