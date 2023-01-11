using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : State
{
  private Player player;

  protected D_PlayerDashState stateData;

  public bool dashing;
  public float timeDashing = 0;

  public PlayerDashState(Entity entity, StateMachine stateMachine, string animBoolName, D_PlayerDashState stateData, Player player

) : base(entity, stateMachine, animBoolName)
  {
    this.player = player;
    this.stateData = stateData;
  }

  public override void Enter()
  {
    base.Enter();

    entity.SetVelocity(stateData.dashSpeed);

    timeDashing = 0;
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
