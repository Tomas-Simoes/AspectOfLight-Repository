using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornborne : Enemy
{
    public Thornborne_ChaseState chaseState { get; private set; }

    [Header("State Data")]
    [SerializeField]
    private D_EnemyChaseState chaseStateData;

    public override void Start()
    {
        base.Start();
        chaseState = new Thornborne_ChaseState(this, stateMachine, "move", chaseStateData, this);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void UpdateCooldowns()
    {
        base.UpdateCooldowns();
    }
}
