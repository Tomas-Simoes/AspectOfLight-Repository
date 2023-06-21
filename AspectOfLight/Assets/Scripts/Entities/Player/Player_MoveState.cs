using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_MoveState : State
{
    private Player player;
    private D_PlayerMoveState stateData;

    public Player_MoveState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_PlayerMoveState stateData,
        Player player
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.player = player;
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.axisMovement == Vector2.zero)
            stateMachine.ChangeState(player.idleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        entity.rb.velocity =
            player.axisMovement.normalized
            * (GameManager.Instance.onGodMode ? 6 : player.currentMovementSpeed);
    }

    public void ChangePlayerMovementSpeed(Dictionary<string, object> message)
    {
        player.currentMovementSpeed = stateData.movementSpeed;
        EventManager.StopListening("onRest", ChangePlayerMovementSpeed);
    }
}
