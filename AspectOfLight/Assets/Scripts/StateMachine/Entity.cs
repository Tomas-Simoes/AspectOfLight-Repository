using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    public StateMachine stateMachine;

    [Header("Entity Data")]
    public D_Entity entityData;

    [SerializeField]
    public float currentHealth;
    public int facingDirection;

    [Header("Entity Informations")]
    [SerializeField]
    private Transform wallCheck;

    public GameObject target;

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
        UpdateCooldowns();
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void UpdateCooldowns() { }

    public void GoTowardsPosition(Vector2 newPosition, float speed)
    {
        rb.MovePosition(
            Vector3.MoveTowards(transform.position, newPosition, speed * Time.fixedDeltaTime)
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
    }

    public GameObject CreatePrefab(
        GameObject prefabToSpawn,
        Vector2 spawnPointPosition,
        GameObject parent
    )
    {
        GameObject _newGameObject = Instantiate(
            prefabToSpawn,
            spawnPointPosition,
            Quaternion.identity
        );
        _newGameObject.transform.parent = parent.transform;

        return _newGameObject;
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
        /*
        Gizmos.DrawLine(
            wallCheck.position,
            wallCheck.position
                + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance)
        );
        */
    }
}
