using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : State
{
  private Player player;

  protected D_PlayerMoveState stateData;
  InputHandler inputHandler;

  private bool dashing = false;

  public PlayerMoveState(
    Entity entity,
    StateMachine stateMachine,
    string animBoolName,
    D_PlayerMoveState stateData,
    Player player
    ) : base(entity, stateMachine, animBoolName)
  {
    this.player = player;
    this.stateData = stateData;
  }

  public override void Enter()
  {
    base.Enter();
    dashing = false;

    if (inputHandler == null)
    {
      inputHandler = entity.GetComponent<InputHandler>();
    }
  }

  public override void Exit()
  {
    base.Exit();
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();
    stateData.axisMovement.x = inputHandler.xAxis;
    stateData.axisMovement.y = inputHandler.yAxis;

    dashing = inputHandler.pressedDash;

    if (inputHandler.pressedDash)
    {
      stateMachine.ChangeState(player.dashState);

    }
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();

    entity.rb.velocity = stateData.axisMovement.normalized * stateData.movementSpeed;
  }
}
