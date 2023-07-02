using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    public StateMachine stateMachine;

    [Header("Entity Data")]
    public GameObject abilityInstances;
    public D_Entity entityData;
    public float currentHealth;
    public int facingDirection;
    public GameObject target;
    public GameObject pivot;

    [Header("Others")]
    private Color defaultDamageColor = Color.red;

    public virtual void Awake()
    {
        stateMachine = new StateMachine();
    }

    public virtual void Start()
    {
        facingDirection = 1;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        abilityInstances = GameObject.FindGameObjectWithTag("AbilityInstances");
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
        if (futurePosition == currentPosition || futurePosition.x == currentPosition.x)
            return;

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

    public virtual void TakeDamage(float damage)
    {
        TakeDamage(damage, Color.red);
    }

    public virtual void TakeDamage(float damage, Color damageColor)
    {
        StartCoroutine(VisualIndicator(damageColor));
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            //TODO play death animation and then Die();
            Die();
        }
    }

    public virtual void TakePoisonDamage(float damage, float poisonTime, float tickRate)
    {
        Coroutine poisonCoroutine = StartCoroutine(Poisoned(damage, tickRate));
        StartCoroutine(StopCoroutineAfterTime(poisonCoroutine, poisonTime));
    }

    protected IEnumerator Poisoned(float damage, float tickRate)
    {
        while (true)
        {
            TakeDamage(damage, Color.magenta);
            yield return new WaitForSeconds(tickRate);
        }
    }

    protected IEnumerator StopCoroutineAfterTime(
        Coroutine courotine,
        float seconds,
        Action callback = null
    )
    {
        yield return new WaitForSeconds(seconds);

        callback?.Invoke();

        StopCoroutine(courotine);
    }

    public virtual void Heal(float healAmount)
    {
        StartCoroutine(VisualIndicator(Color.green));
        currentHealth += healAmount;

        if (currentHealth > entityData.MAX_HEALTH)
            currentHealth = entityData.MAX_HEALTH;
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
            Quaternion.identity,
            parent.transform
        );

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
