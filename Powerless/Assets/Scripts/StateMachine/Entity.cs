using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
  [Header("Entity Data")]
  public StateMachine stateMachine;
  public D_Entity entityData;

  protected float currentHealth;

  public int facingDirection { get; private set; }
  public Rigidbody2D rb { get; private set; }
  public Animator anim { get; private set; }


  [SerializeField]
  private Transform wallCheck;
  private Vector2 velocityWorkspace;

  public virtual void Start()
  {
    facingDirection = 1;

    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();

    stateMachine = new StateMachine();
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

  public virtual void CheckIfFlip(
      int facingDirection,
      Vector2 currentPosition,
      Vector2 futurePosition
  )
  {
    bool _newPatrolPointIsOnLeft = currentPosition.x > futurePosition.x;

    if (facingDirection == 1)
    {
      if (_newPatrolPointIsOnLeft)
        Flip();
    }
    else
    {
      if (!_newPatrolPointIsOnLeft)
        Flip();
    }
  }

  public void OnDeathAnimationFinished()
  {
    Destroy(gameObject);
  }

  public virtual void OnDrawGizmos()
  {
    Gizmos.DrawLine(
        wallCheck.position,
        wallCheck.position
            + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance)
    );
  }

  public virtual void TakeDamage(float damage)
  {
  }
}
