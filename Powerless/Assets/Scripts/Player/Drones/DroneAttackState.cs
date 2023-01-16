using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAttackState : State
{
  private Drone drone;
  protected D_DroneAttackState stateData;
  private D_DroneCooldown coolDownData;

  public DroneAttackState(Entity entity,
  StateMachine stateMachine,
  string animBoolName,
  D_DroneAttackState stateData,
  Drone drone
) : base(entity, stateMachine, animBoolName)
  {
    this.drone = drone;
    this.stateData = stateData;
  }

  public override void Enter()
  {
    base.Enter();
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();
  }

  public override void Exit()
  {
    base.Exit();
  }
}
