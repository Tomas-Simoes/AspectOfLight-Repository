using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultRangedAttackState : State
{
    private Player player;
    private D_PlayerShootingState stateData;

    public PlayerDefaultRangedAttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_PlayerShootingState stateData,
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
        GameObject _bullet = entity.CreatePrefab(
            stateData.bulletPrefab,
            stateData.barrel.transform.position,
            stateData.bulletsParent
        );
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        stateMachine.ChangeState(player.moveState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
