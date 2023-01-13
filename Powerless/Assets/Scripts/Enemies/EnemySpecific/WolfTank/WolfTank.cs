using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfTank : Entity
{
    public WolfTank_IdleState idleState { get; private set; }
    public WolfTank_MoveState moveState { get; private set; }
    public WolfTank_ChaseState chaseState { get; private set; }

    public GameObject[] patrolPoints;

    [SerializeField]
    private D_EnemyIdleState idleStateData;

    [SerializeField]
    private D_EnemyMoveState moveStateData;

    [SerializeField]
    private D_EnemyChaseState chaseStateData;

    [SerializeField]
    private D_EnemyData enemyData;

    public override void Start()
    {
        base.Start();
        idleState = new WolfTank_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new WolfTank_MoveState(this, stateMachine, "move", moveStateData, this);
        chaseState = new WolfTank_ChaseState(
            this,
            stateMachine,
            "move", // this can change if we want to have a "chace" animation aswell
            chaseStateData,
            this,
            enemyData
        );
        stateMachine.Init(moveState);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyData.target = other.gameObject;
            stateMachine.ChangeState(chaseState);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyData.target = null;
            Debug.Log(enemyData.target);
            stateMachine.ChangeState(moveState);
        }
    }
}
