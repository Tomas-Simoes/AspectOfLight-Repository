using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Entity
{
    public GameObject closestEnemy;

    public DroneChaseState chaseState { get; private set; }
    public DroneAttackState attackState { get; private set; }

    //public DroneIdleState droneIdle { get; private set; } ???

    [Header("States Data")]
    [SerializeField]
    private D_DroneAttackState attackStateData;

    [SerializeField]
    private D_DroneChaseState chaseStateData;

    [SerializeField]
    private LayerMask targetableObjects;

    public override void Start()
    {
        base.Start();

        chaseState = new DroneChaseState(
            this,
            stateMachine,
            "chase",
            chaseStateData,
            this,
            attackStateData
        );
        attackState = new DroneAttackState(this, stateMachine, "die", attackStateData, this);

        stateMachine.Init(chaseState);
    }

    public override void Die()
    {
        Collider2D[] _enemiesToDamage = Physics2D.OverlapCircleAll(
            transform.position,
            attackStateData.explosionRange,
            targetableObjects
        );

        foreach (Collider2D _enemy in _enemiesToDamage)
        {
            Debug.Log(_enemy.transform.parent);
            _enemy.GetComponent<Entity>().TakeDamage(attackStateData.damage);
        }

        base.Die();
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, attackStateData.explosionRange);
    }
}
