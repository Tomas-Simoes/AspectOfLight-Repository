using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfKnight : Entity
{
    public GameObject target;

    public WolfKnight_IdleState idleState { get; private set; }
    public WolfKnight_MoveState moveState { get; private set; }
    public WolfKnight_ChaseState chaseState { get; private set; }
    public WolfKnight_AttackState attackState { get; private set; }
    public WolfKnight_RecoveryAttackState recoveryAttackState { get; private set; }

    [Header("Cooldowns")]
    public float timeSinceAttack;

    [Header("State Data")]
    [SerializeField]
    private D_EnemyIdleState idleStateData;

    [SerializeField]
    private D_EnemyMoveState moveStateData;

    [SerializeField]
    private D_EnemyChaseState chaseStateData;

    [SerializeField]
    private D_EnemyMeleeAttackData meleeAttackData;

    [SerializeField]
    private D_EnemyCoolDownData coolDownData;

    [SerializeField]
    private D_EnemyData enemyData;

    [SerializeField]
    private D_EnemyRecoveryAttackState recoveryAttackData;

    [Header("Others")]
    public GameObject[] patrolPoints;
    public GameObject attackPosition;

    public override void Start()
    {
        base.Start();
        currentHealth = enemyData.MAX_HEALTH;
        timeSinceAttack = coolDownData.normalAttackCD;

        idleState = new WolfKnight_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new WolfKnight_MoveState(this, stateMachine, "move", moveStateData, this);
        chaseState = new WolfKnight_ChaseState(
            this,
            stateMachine,
            "move", // this can change if we want to have a "chace" animation aswell
            chaseStateData,
            this,
            enemyData,
            meleeAttackData,
            coolDownData
        );

        attackState = new WolfKnight_AttackState(
            this,
            stateMachine,
            "attack",
            meleeAttackData,
            this
        );

        recoveryAttackState = new WolfKnight_RecoveryAttackState(
            this,
            stateMachine,
            "move",
            recoveryAttackData,
            this,
            meleeAttackData,
            coolDownData,
            moveStateData
        );

        stateMachine.Init(moveState);
    }

    public override void Update()
    {
        base.Update();

        timeSinceAttack += Time.deltaTime;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.transform.position, meleeAttackData.attackRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(target.transform.position, recoveryAttackData.recoveryAreaRadius);

        if (Application.isPlaying)
            Gizmos.DrawLine(transform.position, recoveryAttackState.recoveryPoint);
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
            stateMachine.ChangeState(moveState);
        }
    }
}
