using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WolfKnight : Entity
{
    public WolfKnight_IdleState idleState { get; private set; }
    public WolfKnight_MoveState moveState { get; private set; }
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

    [SerializeField]
    private D_EnemyRecoveryAttackState recoveryAttackData;

    [Header("Cooldowns")]
    public float timeSinceAttack;

    [Header("Pathing")]
    private Seeker seeker;
    public Path path;
    public int currentWaypoint = 0;

    [Header("Informations")]
    public GameObject target;
    public bool goTowardsOtherPatrolPoint;
    public GameObject[] patrolPoints;
    public GameObject attackPosition;

    public override void Start()
    {
        base.Start();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.2f);

        currentHealth = entityData.MAX_HEALTH;
        timeSinceAttack = coolDownData.normalAttackCD;

        idleState = new WolfKnight_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new WolfKnight_MoveState(this, stateMachine, "move", patrolStateData, this);
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
            "move",
            recoveryAttackData,
            this,
            meleeAttackData,
            coolDownData,
            patrolStateData
        );

        stateMachine.Init(moveState);
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

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(pivot.transform.position, target.transform.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            this.path = path;
            currentWaypoint = 0;
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.transform.position, meleeAttackData.attackRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(target.transform.position, recoveryAttackData.recoveryAreaRadius);

        if (Application.isPlaying)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(
                pivot.transform.position,
                meleeAttackData.minimumDistanceToAttack
            );

            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, recoveryAttackState.recoveryPoint);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere((Vector2)path.vectorPath[currentWaypoint], 0.5f);
        }
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
