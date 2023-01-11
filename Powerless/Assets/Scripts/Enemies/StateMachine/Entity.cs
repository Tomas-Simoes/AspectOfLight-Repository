using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EnemyStateMachine stateMachine;
    public D_Entity entityData;

    public int facingDirection { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    public GameObject enemyObj { get; private set; }

    [SerializeField]
    private Transform wallCheck;

    private Vector2 velocityWorkspace;

    public virtual void Start()
    {
        facingDirection = 1;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        stateMachine = new EnemyStateMachine();
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace;
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(
            wallCheck.position,
            gameObject.transform.right,
            entityData.wallCheckDistance,
            entityData.whatIsGround
        );
    }

    public virtual void Flip()
    {
        facingDirection *= -1;
        gameObject.transform.Rotate(0f, 180f, 0f);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            wallCheck.position,
            wallCheck.position
                + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance)
        );
    }

    public virtual void SetCurrentPatrolIndex(int newIndex) { }

    public virtual void MoveTowardsPatrolPoint(GameObject patrolPoint, float movementSpeed)
    {
        rb.MovePosition(
            Vector3.MoveTowards(
                transform.position,
                patrolPoint.transform.position,
                movementSpeed * Time.fixedDeltaTime
            )
        );
    }
}
