using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Thornborne : Enemy
{
    public Thornborne_SleepState sleepState { get; private set; }
    public Thornborne_AwakeState awakeState { get; private set; }
    public Thornborne_ChaseState chaseState { get; private set; }
    public Thornborne_UGAttackState ugAttackState { get; private set; }
    public Thornborne_UGTeleportState uGTeleportState { get; private set; }

    [Header("State Data")]
    [SerializeField]
    private D_EnemyChaseState chaseStateData;

    [SerializeField]
    private D_ThornborneUGAttackState ugAttackStateData;

    [SerializeField]
    private D_ThornborneCooldown cooldownData;

    [Header("Thornborne Information")]
    public bool canTakeDamage;
    public bool fightAlreadyInitiated;

    [Header("Cooldowns")]
    public float timeSinceUGAttack;
    public float timeSinceUGTeleport;

    [Header("Abilities Information")]
    public GameObject ugAttackExplosion;
    public GameObject ugAttackExplosionParent;

    [SerializeField]
    private Transform ugAttackReferencePointsParent;

    public override void Start()
    {
        base.Start();

        sleepState = new Thornborne_SleepState(this, stateMachine, "idle", this);
        awakeState = new Thornborne_AwakeState(this, stateMachine, "awake", this);
        chaseState = new Thornborne_ChaseState(this, stateMachine, "move", chaseStateData, this);
        ugAttackState = new Thornborne_UGAttackState(
            this,
            stateMachine,
            "ugAttack",
            this,
            ugAttackStateData
        );
        uGTeleportState = new Thornborne_UGTeleportState(this, stateMachine, "ugTelport", this);

        InitCooldowns();
        InitReferencePoints();

        stateMachine.Init(sleepState);
    }

    public override void Update()
    {
        base.Update();

        UpdateCooldowns();
        UpdateStates();
    }

    public void UpdateStates()
    {
        if (!isFightStarted)
            return;

        bool _inRangeToUGAbility =
            Vector2.Distance(transform.position, playerReference.transform.position)
            >= ugAttackStateData.ugAttackRange;

        bool _canUGAttack =
            timeSinceUGAttack >= cooldownData.ugAttackCD
            && stateMachine.currentState != ugAttackState
            && _inRangeToUGAbility;

        bool _canUGTeleport =
            timeSinceUGTeleport >= cooldownData.ugTeleportCD
            && timeSinceUGAttack <= cooldownData.ugAttackCD
            && _inRangeToUGAbility;

        if (_canUGAttack)
            stateMachine.ChangeState(ugAttackState);

        if (_canUGTeleport)
            stateMachine.ChangeState(uGTeleportState);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void UpdateCooldowns()
    {
        base.UpdateCooldowns();
        timeSinceUGAttack += Time.deltaTime;
    }

    public override void TakeDamage(float damage)
    {
        if (canTakeDamage)
            base.TakeDamage(damage);
    }

    public void InitCooldowns()
    {
        timeSinceUGAttack = cooldownData.ugAttackCD;
    }

    public void InitReferencePoints()
    {
        List<Transform> _ugAttackReferencePointsList = ugAttackReferencePointsParent
            .Cast<Transform>()
            .ToList();

        ugAttackState.referencePoints = _ugAttackReferencePointsList.ConvertAll(
            referencePoint => new Vector2(referencePoint.position.x, referencePoint.position.y)
        );
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, ugAttackStateData.ugAttackRange);
    }
}
