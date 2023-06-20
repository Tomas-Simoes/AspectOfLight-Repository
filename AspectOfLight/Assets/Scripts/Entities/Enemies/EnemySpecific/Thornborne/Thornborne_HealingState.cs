using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornborne_HealingState : State
{
    Thornborne enemy;
    D_ThornborneHealingState stateData;

    [Header("State Information")]
    private float timeSinceHealing;

    public Thornborne_HealingState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        Thornborne enemy,
        D_ThornborneHealingState stateData
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.rb.velocity = Vector2.zero;
        timeSinceHealing = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        timeSinceHealing += Time.deltaTime;

        if (timeSinceHealing >= stateData.timeToHeal)
        {
            enemy.Heal(stateData.heal);
            timeSinceHealing = 0;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
