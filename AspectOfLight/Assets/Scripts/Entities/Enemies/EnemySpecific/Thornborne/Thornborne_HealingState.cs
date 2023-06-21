using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornborne_HealingState : State
{
    Thornborne enemy;
    D_ThornborneHealingState stateData;

    [Header("State Information")]
    private float timeSinceLastHealingTick;

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
        timeSinceLastHealingTick = stateData.timeToHeal;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.timeSinceHealing = 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        timeSinceLastHealingTick += Time.deltaTime;

        if (timeSinceLastHealingTick >= stateData.timeToHeal)
        {
            enemy.Heal(stateData.heal);
            timeSinceLastHealingTick = 0;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
