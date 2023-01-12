using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
  public PlayerMoveState moveState { get; private set; }
  public PlayerDashState dashState { get; private set; }
  public InputHandler inputHandler { get; private set; }
  
  [SerializeField]
  private D_PlayerCoolDown coolDownData;

  [SerializeField]
  private D_PlayerMoveState moveStateData;

  [SerializeField]
  private D_PlayerDashState dashStateData;

  public override void Start()
  {
    base.Start();

    if (inputHandler == null)
    {
      inputHandler = this.GetComponent<InputHandler>();
    }

    moveState = new PlayerMoveState(this, stateMachine, "move", moveStateData, this, coolDownData);
    dashState = new PlayerDashState(this, stateMachine, "dash", dashStateData, this, coolDownData);
    stateMachine.Init(moveState);
  }
}
