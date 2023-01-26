using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity Data")]
    public StateMachine stateMachine;
    public D_Entity entityData;

    [Header("Entity Data")]
    [SerializeField]
    protected float currentHealth;
    public int facingDirection;

    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    [SerializeField]
    private Transform wallCheck;

    [SerializeField]
    public GameObject pivot;

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

    public void GoTowardsTarget(Vector2 targetPosition, float speed)
    {
        rb.MovePosition(
            Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime)
        );
    }

    public virtual void Flip()
    {
        facingDirection *= -1;
        gameObject.transform.Rotate(0f, 180f, 0f);
    }

    public void CheckIfFlip(int facingDirection, Vector2 currentPosition, Vector2 futurePosition)
    {
        bool _newPositionIsOnLeft = currentPosition.x > futurePosition.x;

        if (facingDirection == 1)
        {
            if (_newPositionIsOnLeft)
                Flip();
        }
        else
        {
            if (!_newPositionIsOnLeft)
                Flip();
        }
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(VisualIndicator(Color.red));
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            //play death animation and then Die();
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);

    //Change this later to Enemy class
    public GameObject InstantiateObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return Instantiate(prefab, position, rotation);
    }

    //Only for testing purpouses

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(
            wallCheck.position,
            gameObject.transform.right,
            entityData.wallCheckDistance,
            entityData.whatIsGround
        );
    }

    public void WaitForAnimationToFinish(float animationDuration)
    {
        StartCoroutine(Wait(animationDuration));
    }

    public IEnumerator Wait(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
    }

    private IEnumerator VisualIndicator(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            wallCheck.position,
            wallCheck.position
                + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance)
        );
    }
}
