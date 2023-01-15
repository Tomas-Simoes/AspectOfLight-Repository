using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneChaseState : State
{
     public GameObject[] multiEnemys;
     public Drone drone;

     protected D_DroneChaseState stateData;
     protected bool isDetectingWall;
     protected D_DroneChaseState StateData;
     private D_DroneCooldown coolDownData;

      public DroneChaseState(Entity entity, 
      StateMachine stateMachine,
      string animBoolName,
      D_DroneChaseState stateData,
      Drone drone,
      D_DroneCooldown coolDownData

) : base(entity, stateMachine, animBoolName)
  {
    this.drone = drone;
    this.StateData = stateData;
    this.coolDownData = coolDownData;
  }


  public override void Enter()
  {
        entity.SetVelocity(stateData.droneChaseSpeed);

        isDetectingWall = entity.CheckWall();


        GameObject[] EnemyArray;
        EnemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = entity.transform.position;
        foreach (GameObject gameobject in EnemyArray)
        {
            Vector3 diff = gameobject.transform.position - position;
            float currentDistance = diff.sqrMagnitude;
            if (currentDistance < distance)
            {
                closest = gameobject;
                distance = currentDistance;
            }
        }
    base.Enter();
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
  }

  public override void PhysicsUpdate()
  { 

    base.PhysicsUpdate();
  }

  public override void UpdateCooldowns()
  {
    base.UpdateCooldowns();
  }

      public virtual void StartChasing(GameObject _closestTarget)
    {
        entity.rb.MovePosition(
            Vector3.MoveTowards(
                entity.transform.position,
                _closestTarget.transform.position,
                stateData.droneChaseSpeed * Time.fixedDeltaTime
            )
        );
    }

}
