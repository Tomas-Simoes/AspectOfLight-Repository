using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : State
{
  private Player player;

  private D_PlayerMoveState stateData;
  private D_PlayerCoolDown coolDownData;

  private bool dashing = false;

  public PlayerMoveState( Entity entity,
      StateMachine stateMachine,
      string animBoolName,
      D_PlayerMoveState stateData, 
      Player player, D_PlayerCoolDown
      coolDownData
    ) : base(entity, stateMachine, animBoolName)
  {
    this.player = player;
    this.stateData = stateData;
    this.coolDownData = coolDownData;
  }

  public override void Enter()
  {
    base.Enter();
    dashing = false;
  }

  public override void Exit()
  {
    base.Exit();
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();
    stateData.axisMovement.x = player.inputHandler.xAxis;
    stateData.axisMovement.y = player.inputHandler.yAxis;

    dashing = player.inputHandler.pressedDash;

    if (player.inputHandler.pressedDash && coolDownData.timeSinceDash >= coolDownData.dashCD)
    {
      stateMachine.ChangeState(player.dashState);
    }
    
    GetInputs();
    UpdateCooldowns();
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();

    entity.rb.velocity = stateData.axisMovement.normalized * stateData.movementSpeed;
  }

  public override void UpdateCooldowns()
  {
    base.UpdateCooldowns();

    coolDownData.timeSinceDash += Time.deltaTime;
  }

  public override void GetInputs()
  {
    base.GetInputs();

    if (player.inputHandler.pressedDash && coolDownData.timeSinceDash >= coolDownData.dashCD)
    {
      stateMachine.ChangeState(player.dashState);
    }

    if(player.inputHandler.pressedAbilityOne){
      stateMachine.ChangeState(player.necromancerState);
    }
  }
}
