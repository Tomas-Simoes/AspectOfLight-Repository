using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfTank : Enemy
{
    public WolfTank_IdleState idleState { get; private set; }
    public WolfTank_PatrolState patrolState { get; private set; }
    public WolfTank_ChaseState chaseState { get; private set; }

    [Header("State Data")]
    [SerializeField]
    private D_EnemyIdleState idleStateData;

    [SerializeField]
    private D_EnemyPatrolState patrolStateData;

    [SerializeField]
    private D_EnemyChaseState chaseStateData;

    [Header("Cooldowns")]
    public float timeSinceAttack;

    [Header("Informations")]
    public GameObject attackPosition;

    public override void Start()
    {
        base.Start();

        idleState = new WolfTank_IdleState(this, stateMachine, "idle", idleStateData, this);
        patrolState = new WolfTank_PatrolState(this, stateMachine, "move", patrolStateData, this);
        chaseState = new WolfTank_ChaseState(
            this,
            stateMachine,
            "move", // this can change if we want to have a "chace" animation aswell
            chaseStateData,
            this
        );
        stateMachine.Init(patrolState);
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
            stateMachine.ChangeState(patrolState);
        }
    }
}
