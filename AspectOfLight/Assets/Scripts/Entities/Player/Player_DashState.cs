using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DashState : State
{
    private Player player;
    protected D_PlayerDashState stateData;

    private bool dashing;
    private float timeDashing = 0;

    public Player_DashState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_PlayerDashState stateData,
        Player player,
        D_PlayerCoolDown coolDownData
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.player = player;
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        player.ReduceStamina(stateData.staminaCost);

        float _xAxis = player.inputHandler.xAxis;
        float _yAxis = player.inputHandler.yAxis;

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
            player.timeSinceDash = 0;

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

        entity.rb.velocity = player.axisMovement.normalized * stateData.dashSpeed;
    }
}
