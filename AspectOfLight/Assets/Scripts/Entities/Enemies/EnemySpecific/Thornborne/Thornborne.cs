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
    public Thornborne_PukeAttackState pukeAttackState { get; private set; }
    public Thornborne_HealingState healingState { get; private set; }

    [Header("State Data")]
    [SerializeField]
    private D_EnemyChaseState chaseStateData;

    public D_ThornborneUGAttackState ugAttackStateData;

    public D_ThornborneCooldown cooldownData;

    public D_ThornbornePukeAttackState pukeAttackStateData;

    public D_ThornborneUGTeleportState ugTeleportStateData;

    [SerializeField]
    private D_ThornborneHealingState healingStateData;

    [Header("Thornborne Information")]
    public bool canTakeDamage;
    public bool fightAlreadyInitiated;

    [Header("Cooldowns")]
    public float timeSinceUGAttack;
    public float timeSinceUGTeleport;
    public float timeSincePukeAttack;
    public float timeSinceHealing;

    [Header("Abilities Information")]
    public GameObject abilitiesParent;

    [Space(2)]
    public GameObject ugAttackExplosion;

    [SerializeField]
    private Transform ugAttackReferencePointsParent;

    [Space(2)]
    public GameObject shootingPlant;

    [Space(2)]
    public GameObject pukeAttack;
    public GameObject pukeAttackPosition;

    [Space(2)]
    [SerializeField]
    public GameObject[] safePoints;

    public override void Start()
    {
        base.Start();

        sleepState = new Thornborne_SleepState(this, stateMachine, "sleep", this);
        awakeState = new Thornborne_AwakeState(this, stateMachine, "awake", this);
        chaseState = new Thornborne_ChaseState(
            this,
            stateMachine,
            "move",
            chaseStateData,
            cooldownData,
            this
        );
        ugAttackState = new Thornborne_UGAttackState(
            this,
            stateMachine,
            "ugAttack",
            this,
            ugAttackStateData
        );
        uGTeleportState = new Thornborne_UGTeleportState(
            this,
            stateMachine,
            "ugTeleport",
            this,
            ugTeleportStateData
        );
        pukeAttackState = new Thornborne_PukeAttackState(
            this,
            stateMachine,
            "puke",
            this,
            pukeAttackStateData
        );
        healingState = new Thornborne_HealingState(
            this,
            stateMachine,
            "healing",
            this,
            healingStateData
        );

        timeSinceUGAttack = cooldownData.ugAttackCD;
        timeSinceUGTeleport = cooldownData.ugTeleportCD;
        timeSinceHealing = cooldownData.healingCD;

        InitReferencePoints();

        stateMachine.Init(sleepState);
    }

    public override void Update()
    {
        base.Update();

        UpdateCooldowns();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void UpdateCooldowns()
    {
        base.UpdateCooldowns();

        timeSinceUGAttack += Time.deltaTime;
        timeSinceUGTeleport += Time.deltaTime;
        timeSincePukeAttack += Time.deltaTime;
        timeSinceHealing += Time.deltaTime;
    }

    public override void TakeDamage(float damage)
    {
        if (stateMachine.currentState == healingState)
        {
            stateMachine.ChangeState(chaseState);
            return;
        }

        if (canTakeDamage)
            base.TakeDamage(damage);
        else
            Heal(damage);
    }

    public override void Die()
    {
        EventManager.TriggerEvent("onThornborneDie", false, null);

        base.Die();
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

    public void SpawnPuke()
    {
        CreatePrefab(pukeAttack, pukeAttackPosition.transform.position, abilitiesParent);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ugAttackStateData.ugAttackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pukeAttackStateData.pukeAttackRange);
    }

    public void StartSpawningSpikes()
    {
        ugAttackState.canSpawnWaves = true;
    }
}
