using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornborne : Enemy
{
    public Thornborne_ChaseState chaseState { get; private set; }
    public Thornborne_UGAttackState ugAttackState { get; private set; }

    [Header("State Data")]
    [SerializeField]
    private D_EnemyChaseState chaseStateData;

    [SerializeField]
    private D_ThornborneUGAttackState ugAttackStateData;

    [SerializeField]
    private D_ThornborneCooldown cooldownData;

    [Header("Cooldowns")]
    public float timeSinceUGAttack;

    [Header("Abilities Information")]
    public GameObject ugAttackExplosion;
    public GameObject ugAttackExplosionParent;
    public List<Vector2> ugAttackReferencePoints;

    public override void Start()
    {
        base.Start();
        InitCooldowns();

        chaseState = new Thornborne_ChaseState(this, stateMachine, "move", chaseStateData, this);
        ugAttackState = new Thornborne_UGAttackState(
            this,
            stateMachine,
            "ugAttack",
            this,
            ugAttackStateData
        );

        stateMachine.Init(chaseState);
    }

    public override void Update()
    {
        base.Update();

        UpdateCooldowns();
        UpdateStates();
    }

    public void UpdateStates()
    {
        bool _canUGAttack =
            timeSinceUGAttack >= cooldownData.ugAttackCD
            && Vector2.Distance(transform.position, playerReference.transform.position)
                <= ugAttackStateData.ugAttackRange;

        if (_canUGAttack)
        {
            stateMachine.ChangeState(ugAttackState);
        }
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

    public void InitCooldowns()
    {
        timeSinceUGAttack = cooldownData.ugAttackCD;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, ugAttackStateData.ugAttackRange);
    }
}
