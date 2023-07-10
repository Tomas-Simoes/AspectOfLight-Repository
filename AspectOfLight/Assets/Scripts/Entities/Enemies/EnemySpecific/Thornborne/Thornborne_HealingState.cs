using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornborne_HealingState : State
{
    bool exitingHealing = false;

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

        Debug.Log("entered healing");
        enemy.rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.timeSinceHealing = 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (entity.anim.GetCurrentAnimatorStateInfo(0).IsName("HealingDown"))
        {
            float _animationTime = entity.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (_animationTime >= 1)
            {
                timeSinceLastHealingTick = stateData.timeToHeal;
                Debug.Log("ready to heal");
            }

            return;
        }

        if (entity.anim.GetCurrentAnimatorStateInfo(0).IsName("HealingUp"))
        {
            float _animationTime = entity.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (_animationTime >= 1)
            {
                entity.anim.SetBool("healingUp", false);
                stateMachine.ChangeState(enemy.chaseState);
            }

            return;
        }

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
