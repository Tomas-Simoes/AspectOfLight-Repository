using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plague : Entity
{
    public PlagueChaseState chaseState { get; private set; }
    public PlagueAttackState attackState { get; private set; }

    [Header("States Data")]
    [SerializeField]
    private D_PlagueAttackState attackStateData;
    [SerializeField]
    private D_PlagueChaseState chaseStateData;

    [Header("Information")]
    [SerializeField]
    private LayerMask targetableObjects;
    public GameObject closestEnemy;

    public override void Start()
    {
        base.Start();
        facingDirection = -1; // just because the sprite was made facing left

        chaseState = new PlagueChaseState(
            this,
            stateMachine,
            "chase",
            chaseStateData,
            this,
            attackStateData
        );
        attackState = new PlagueAttackState(this, stateMachine, "die", attackStateData, this);

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
            if (_enemy != null)
                _enemy.gameObject.GetComponent<Entity>().TakeDamage(attackStateData.damage);
        }

        base.Die();
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, attackStateData.explosionRange);
    }
}
