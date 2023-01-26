using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight_AttackState : EnemyMeleeAttack
{
    private WolfKnight enemy;
    private D_EnemyMeleeAttackData stateData;

    public WolfKnight_AttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_EnemyMeleeAttackData stateData,
        WolfKnight enemy
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        this.enemy = enemy;
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
        // TODO find a way to stop the code until the animation is completed
        for (
            float i = 0;
            i <= enemy.anim.GetCurrentAnimatorStateInfo(0).length;
            i += Time.deltaTime
        ) { }

        enemy.anim.SetBool("attack", false);

        Collider2D[] objectsOverlaped = Physics2D.OverlapCircleAll(
            enemy.attackPosition.transform.position,
            stateData.attackRadius,
            stateData.attackableLayer
        );

        foreach (Collider2D objectHitted in objectsOverlaped)
        {
            Debug.Log("This got hitted" + objectHitted);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
