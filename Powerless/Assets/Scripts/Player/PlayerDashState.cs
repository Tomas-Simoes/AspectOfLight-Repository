using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : State
{
  private Player player;

  protected D_PlayerDashState stateData;
  private D_PlayerCoolDown coolDownData;
  public bool dashing;
  public float timeDashing = 0;

  public PlayerDashState(Entity entity, 
      StateMachine stateMachine,
      string animBoolName,
      D_PlayerDashState stateData,
      Player player,
      D_PlayerCoolDown coolDownData

) : base(entity, stateMachine, animBoolName)
  {
    this.player = player;
    this.stateData = stateData;
    this.coolDownData = coolDownData;
  }

  public override void Enter()
  {
    base.Enter();

    float _xAxis = player.inputHandler.xAxis;
    float _yAxis = player.inputHandler.yAxis;

    entity.rb.velocity = new Vector2(_xAxis, _yAxis) * stateData.dashSpeed;

    dashing = true;
  }

  public override void Exit()
  {
    base.Exit();
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();
    timeDashing += Time.deltaTime;

    if (timeDashing >= stateData.dashTime)
    {
      timeDashing = 0;
      coolDownData.timeSinceDash = 0;

      dashing = false;
    }

    if (!dashing)
    {
      stateMachine.ChangeState(player.moveState);
    }
  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();
  }
}
