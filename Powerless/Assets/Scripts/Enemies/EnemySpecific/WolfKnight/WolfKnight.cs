using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight : Entity
{
    public WolfKnight_IdleState idleState { get; private set; }
    public WolfKnight_MoveState moveState { get; private set; }

    public GameObject[] patrolPoints;
    public int currentPatrolIndex = 0;

    [SerializeField]
    private D_EnemyIdleState idleStateData;

    [SerializeField]
    private D_EnemyMoveState moveStateData;

    public override void Start()
    {
        base.Start();

        moveState = new WolfKnight_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new WolfKnight_IdleState(this, stateMachine, "idle", idleStateData, this);

        stateMachine.Init(moveState);
    }

    public override void SetCurrentPatrolIndex(int newIndex)
    {
        base.SetCurrentPatrolIndex(newIndex);

        currentPatrolIndex = newIndex;
    }

    public override void MoveTowardsPatrolPoint(GameObject patrolPoint, float movementSpeed)
    {
        base.MoveTowardsPatrolPoint(patrolPoint, movementSpeed);
    }
}
