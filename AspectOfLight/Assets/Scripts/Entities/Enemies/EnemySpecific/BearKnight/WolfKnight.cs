using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WolfKnight : Enemy
{
    public WolfKnight_IdleState idleState { get; private set; }
    public WolfKnight_PatrolState patrolState { get; private set; }
    public WolfKnight_ChaseState chaseState { get; private set; }
    public WolfKnight_AttackState attackState { get; private set; }
    public WolfKnight_RecoveryAttackState recoveryAttackState { get; private set; }

    [Header("State Data")]
    [SerializeField]
    private D_EnemyIdleState idleStateData;

    [SerializeField]
    private D_EnemyPatrolState patrolStateData;

    [SerializeField]
    private D_EnemyChaseState chaseStateData;

    [SerializeField]
    private D_EnemyMeleeAttackState meleeAttackData;

    [SerializeField]
    private D_EnemyCoolDown coolDownData;

    [Header("Wolf Knight Data")]
    public GameObject attackPosition;

    [Header("Wolf Knight Cooldowns")]
    public float timeSinceAttack;

    public override void Start()
    {
        base.Start();
        timeSinceAttack = coolDownData.normalAttackCD;

        idleState = new WolfKnight_IdleState(this, stateMachine, "idle", idleStateData, this);
        patrolState = new WolfKnight_PatrolState(this, stateMachine, "move", patrolStateData, this);
        chaseState = new WolfKnight_ChaseState(
            this,
            stateMachine,
            "move", // this can change if we want to have a "chace" animation aswell
            chaseStateData,
            this,
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
            "idle",
            this,
            meleeAttackData,
            coolDownData
        );

        stateMachine.Init(patrolState);
    }

    public override void Update()
    {
        base.Update();
        UpdateCooldowns();
    }

    public override void UpdateCooldowns()
    {
        base.UpdateCooldowns();
        timeSinceAttack += Time.deltaTime;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.transform.position, meleeAttackData.attackRadius);

        if (Application.isPlaying)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(
                pivot.transform.position,
                meleeAttackData.minimumDistanceToAttack
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            stateMachine.ChangeState(chaseState);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            stateMachine.ChangeState(patrolState);
        }
    }
}
