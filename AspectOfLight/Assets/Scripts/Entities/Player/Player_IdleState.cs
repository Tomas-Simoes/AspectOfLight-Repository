using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_IdleState : State
{
    private Player player;

    public Player_IdleState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        Player player
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.player = player;
    }

    public override void Enter()
    {
        base.Enter();

        entity.rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.axisMovement != Vector2.zero)
            stateMachine.ChangeState(player.moveState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
