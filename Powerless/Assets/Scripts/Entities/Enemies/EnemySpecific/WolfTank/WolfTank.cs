using UnityEngine;

public class WolfTank : Enemy
{
    public WolfTank_IdleState idleState { get; private set; }
    public WolfTank_PatrolState patrolState { get; private set; }
    public WolfTank_ChaseState chaseState { get; private set; }
    public WolfTank_AttackState attackState { get; private set; }

    [Header("State Data")]
    [SerializeField]
    private D_EnemyIdleState idleStateData;

    [SerializeField]
    private D_EnemyPatrolState patrolStateData;

    [SerializeField]
    private D_EnemyRangedAttackState rangedAttackData;

    [SerializeField]
    private D_EnemyChaseState chaseStateData;

    [SerializeField]
    private D_EnemyCoolDown coolDownData;

    [Header("Cooldowns")]
    public float timeSinceAttack;

    [Header("Informations")]
    [SerializeField]
    public GameObject bulletPrefab;

    [SerializeField]
    public GameObject bulletsParent;
    public GameObject attackPosition;

    public override void Start()
    {
        base.Start();
        timeSinceAttack = coolDownData.normalAttackCD;

        idleState = new WolfTank_IdleState(this, stateMachine, "idle", idleStateData, this);
        patrolState = new WolfTank_PatrolState(this, stateMachine, "move", patrolStateData, this);
        chaseState = new WolfTank_ChaseState(
            this,
            stateMachine,
            "move", // this can change if we want to have a "chace" animation aswell
            chaseStateData,
            this,
            rangedAttackData,
            coolDownData
        );

        attackState = new WolfTank_AttackState(
            this,
            stateMachine,
            "attack",
            rangedAttackData,
            this
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
        if (Application.isPlaying)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(
                pivot.transform.position,
                rangedAttackData.minimumDistanceToAttack
            );
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
