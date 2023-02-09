using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : State
{
    private Player player;
    private D_PlayerMoveState stateData;

    private bool FacingRight = true; //Flip stuff

    public PlayerMoveState(
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        entity.rb.velocity = player.axisMovement.normalized * stateData.movementSpeed;

        Vector2 dir =
            Input.mousePosition - Camera.main.WorldToScreenPoint(player.transform.position);

        //Check "Direction" (dir), and the flips.
        if (dir.x > 0 && !FacingRight || dir.x < 0 && FacingRight)
        {
            CheckForFlipping();
        }
    }

    private void CheckForFlipping()
    {
        FacingRight = !FacingRight;
        player.transform.Rotate(0f, 180f, 0f);
    }
}
