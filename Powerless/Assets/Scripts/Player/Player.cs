using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
  public PlayerMoveState moveState { get; private set; }
  public PlayerDashState dashState { get; private set; }

  [SerializeField]
  private D_PlayerMoveState moveStateData;

  [SerializeField]
  private D_PlayerDashState dashStateData;
  public override void Start()
  {
    base.Start();

    moveState = new PlayerMoveState(this, stateMachine, "move", moveStateData, this);
    dashState = new PlayerDashState(this, stateMachine, "dash", dashStateData, this);
    stateMachine.Init(moveState);
  }
}
