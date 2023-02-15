using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    public Skeleton_idleState idleState { get; private set; }
    public Skeleton_PatrolState patrolState { get; private set; }
    public Skeleton_ChaseState chaseState { get; private set; }
    public Skeleton_AttackState attackState { get; private set; }
    public Skeleton_RecoveryState recoveryState { get; private set; }

    [Header("State Data")]
    [SerializeField]
    private D_EnemyIdleState idleStateData;

    [SerializeField]
    private D_EnemyPatrolState patrolStateData;

    [SerializeField]
    private D_EnemyChaseState chaseStateData;

    [SerializeField]
    private D_EnemyRangedAttackState rangedStateData;

    [SerializeField]
    private D_EnemyRecoveryAttackState recoveryAttackData;

    [SerializeField]
    private D_EnemyCoolDown coolDownData;

    [Header("Cooldowns")]
    public float timeSinceAttack;

    [Header("Informations")]
    [SerializeField]
    public GameObject bulletPrefab;

    [SerializeField]
    public GameObject bulletsParent;
    [SerializeField]
    public GameObject attackPosition;

    //  public GameObject attackPosition;
    public override void Start()
    {
        base.Start();
        currentHealth = entityData.MAX_HEALTH;
        timeSinceAttack = coolDownData.normalAttackCD;

        idleState = new Skeleton_idleState(this, stateMachine, "idle", idleStateData, this);
        patrolState = new Skeleton_PatrolState(this, stateMachine, "move", patrolStateData, this);
        chaseState = new Skeleton_ChaseState(
            this,
            stateMachine,
            "move", // this can change if we want to have a "chace" animation aswell
            chaseStateData,
            this,
            rangedStateData,
            coolDownData
        );

        attackState = new Skeleton_AttackState(this, stateMachine, "attack", rangedStateData, this);

        recoveryState = new Skeleton_RecoveryState(
            this,
            stateMachine,
            "idle",
            this,
            rangedStateData,
            coolDownData,
            patrolStateData
        );

        stateMachine.Init(patrolState);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void UpdateCooldowns()
    {
        base.UpdateCooldowns();
        timeSinceAttack += Time.deltaTime;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(rb.transform.position, rangedStateData.minimumDistanceToAttack); //TODO Change this to draw with the center on the pivot
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
            target = null;
            stateMachine.ChangeState(patrolState);
        }
    }
}
