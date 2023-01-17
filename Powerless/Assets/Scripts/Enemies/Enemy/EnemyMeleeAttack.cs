using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : State
{
    protected bool attackedOnce = false;

    public EnemyMeleeAttack(Entity entity, StateMachine stateMachine, string animBoolName)
        : base(entity, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        entity.SetVelocity(0f);
    }
}
