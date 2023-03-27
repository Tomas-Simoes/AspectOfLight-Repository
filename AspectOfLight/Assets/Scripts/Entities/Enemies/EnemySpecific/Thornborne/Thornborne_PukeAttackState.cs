using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornborne_PukeAttackState : State
{
    //TODO when the animation is done update the '_animationTime' to use the GetCurrentAnimatorStateInfo

    private Thornborne enemy;
    private D_ThornbornePukeAttackState stateData;

    private float _animationTime;

    public Thornborne_PukeAttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        Thornborne enemy,
        D_ThornbornePukeAttackState stateData
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        _animationTime = 0;

        enemy.rb.velocity = Vector2.zero;
        enemy.timeSincePukeAttack = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        _animationTime += Time.deltaTime;

        if (_animationTime >= 1f)
        {
            SpawnPuke();

            stateMachine.ChangeState(enemy.chaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SpawnPuke()
    {
        entity.CreatePrefab(
            enemy.pukeAttack,
            enemy.pukeAttackPosition.transform.position,
            enemy.abilitiesParent
        );
    }
}
