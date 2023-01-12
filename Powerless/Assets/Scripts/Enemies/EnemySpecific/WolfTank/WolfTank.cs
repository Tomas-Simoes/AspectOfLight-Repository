using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfTank : Entity
{
    public WolfTank_IdleState idleState { get; private set; }
    public WolfTank_MoveState moveState { get; private set; }

    public GameObject[] patrolPoints;

    [SerializeField]
    private D_EnemyIdleState idleStateData;

    [SerializeField]
    private D_EnemyMoveState moveStateData;

    public override void Start()
    {
        base.Start();

        idleState = new WolfTank_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new WolfTank_MoveState(this, stateMachine, "move", moveStateData, this);

        stateMachine.Init(moveState);
    }
}
