using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : Enemy
{
    public WalkerIdleState idleState { get; private set; }
    public WalkerPatrolState patrolState { get; private set; }

    [Header("State Data")]
    [SerializeField]
    private D_EnemyIdleState idleStateData;

    [SerializeField]
    private D_EnemyPatrolState patrolStateData;

    public override void Start()
    {
        base.Start();

        idleState = new WalkerIdleState(this, stateMachine, "idle", idleStateData, this);
        patrolState = new WalkerPatrolState(this, stateMachine, "move", patrolStateData, this);

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

