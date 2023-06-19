using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CastingAbilityState : State
{
    public Ability currentAbility;

    public Player_CastingAbilityState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        Ability currentAbility
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.currentAbility = currentAbility;
    }

    public override void Enter()
    {
        base.Enter();

        entity.rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();

        currentAbility = null;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
