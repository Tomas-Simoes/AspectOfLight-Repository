using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornbornePlant : Entity
{
    public ThornbornePlant_ShootingState shootingState { get; private set; }
    public ThornbornePlant_GrowingState growingState { get; private set; }
    public ThornbornePlant_IdleState idleState { get; private set; }

    [Header("Shooting Plant Information")]
    public float shootingCD;
    public GameObject bullet;
    public Transform shootingPosition;

    public GameObject plantTarget;
    public GameObject bulletParent;

    private void OnEnable()
    {
        EventManager.StartListening("onThornborneDie", Die);
    }

    private void OnDisable()
    {
        EventManager.StartListening("onThornborneDie", Die);
    }

    public override void Start()
    {
        base.Start();

        growingState = new ThornbornePlant_GrowingState(this, stateMachine, "growing", this);
        shootingState = new ThornbornePlant_ShootingState(this, stateMachine, "shoot", this);
        idleState = new ThornbornePlant_IdleState(this, stateMachine, "idle", this);

        stateMachine.Init(growingState);
    }

    public override void Update()
    {
        base.Update();

        CheckIfFlip(facingDirection, plantTarget.transform.position, rb.position);
    }

    public void Die(Dictionary<string, object> message)
    {
        base.Die();
    }
}
