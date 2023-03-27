using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornborne_UGTeleportState : State
{
    //TODO change this to animation time

    private Thornborne enemy;
    private float animationTime;

    public Thornborne_UGTeleportState(
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

        enemy.rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        animationTime += Time.deltaTime;

        if (animationTime >= 1)
        {
            TeleportToPlayer();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void TeleportToPlayer()
    {
        Transform _playerTransform = enemy.playerReference.transform;

        Vector3 _newPosition =
            _playerTransform.position
            + new Vector3(_playerTransform.localScale.x + 1f, 0f, 0f)
                * enemy.playerReference.GetComponent<Entity>().facingDirection;

        enemy.rb.MovePosition(_newPosition);

        stateMachine.ChangeState(enemy.chaseState); //TODO change this to melee attack state
    }
}
