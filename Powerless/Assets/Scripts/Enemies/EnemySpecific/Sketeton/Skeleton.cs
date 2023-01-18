using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Entity
{
    public GameObject target;

    public SkeletonidleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonChaseState chaseState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }

    [Header("State Data")]
    [SerializeField]
    private D_EnemyIdleState idleStateData;

    [SerializeField]
    private D_EnemyMoveState moveStateData;

    [SerializeField]
    private D_EnemyChaseState chaseStateData;

    [SerializeField]
    private D_EnemyShootingAttack shootingStateData;

    [SerializeField]
    private D_EnemyCoolDownData coolDownData;

    [SerializeField]
    private D_EnemyData enemyData;

    [Header("Others")]
    public GameObject[] patrolPoints;

    //  public GameObject attackPosition;
    public override void Start()
    {
        base.Start();
        currentHealth = enemyData.MAX_HEALTH;

        idleState = new SkeletonidleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new SkeletonMoveState(
            this,
            stateMachine,
            "move",
            moveStateData,
            this,
            enemyData
        );
        chaseState = new SkeletonChaseState(
            this,
            stateMachine,
            "move", // this can change if we want to have a "chace" animation aswell
            chaseStateData,
            this,
            enemyData,
            shootingStateData,
            coolDownData
        );

        attackState = new SkeletonAttackState(
            this,
            stateMachine,
            "attack",
            shootingStateData,
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
