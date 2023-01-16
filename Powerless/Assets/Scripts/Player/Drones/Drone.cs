using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Entity
{

  public DroneChaseState chaseState { get; private set; }
  public DroneAttackState attackState { get; private set; }
  //public DroneIdleState droneIdle { get; private set; } ???

  [Header("States Data")]
  [SerializeField]
  private D_DroneAttackState attackStateData;

  [SerializeField]
  private D_DroneChaseState chaseStateData;

  [SerializeField]
  private D_DroneCooldown coolDownData;

  public override void Start()
  {
    base.Start();
    chaseState = new DroneChaseState(this, stateMachine, "chase", chaseStateData, this, coolDownData, attackStateData);
    attackState = new DroneAttackState(this, stateMachine, "die", attackStateData, this);
    stateMachine.Init(chaseState);
  }
}
