using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfTank : Entity
{
    public WolfTank_IdleState idleState { get; private set; }
    public WolfTank_MoveState moveState { get; private set; }
    public WolfTank_ChaseState chaseState { get; private set; }

    [Header("State Data")]
    [SerializeField]
    private D_EnemyIdleState idleStateData;

    [SerializeField]
    private D_EnemyPatrolState patrolStateData;

    [SerializeField]
    private D_EnemyChaseState chaseStateData;

    [Header("Informations")]
    public bool goTowardsOtherPatrolPoint;
    public GameObject[] patrolPoints;
    public GameObject attackPosition;

    public override void Start()
    {
        base.Start();
        currentHealth = entityData.MAX_HEALTH;

        idleState = new WolfTank_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new WolfTank_MoveState(this, stateMachine, "move", patrolStateData, this);
        chaseState = new WolfTank_ChaseState(
            this,
            stateMachine,
            "move", // this can change if we want to have a "chace" animation aswell
            chaseStateData,
            this
        );
        stateMachine.Init(moveState);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.gameObject;
            stateMachine.ChangeState(chaseState);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
            stateMachine.ChangeState(moveState);
        }
    }
}
