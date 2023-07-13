using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornborne_SleepState : State
{
    private Thornborne enemy;

    public Thornborne_SleepState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        Thornborne enemy
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.canTakeDamage = false;

        enemy.target = enemy.playerReference;
        enemy.rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.isFightStarted)
        {
            Debug.Log("ya");
            stateMachine.ChangeState(enemy.awakeState);
        }
    }
}
