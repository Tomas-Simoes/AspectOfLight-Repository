using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Entity
{

    public DroneChaseState ChaseState { get; private set; }
    public DroneAttackState AttackState { get; private set; }
    //public DroneIdleState droneIdle { get; private set; } ???

    [Header("States Data")]
    [SerializeField]
    private D_DroneAttackState attackState;

    [SerializeField]
    private D_DroneChaseState chaseState;

    [SerializeField]
    private D_DroneCooldown coolDownData;

        public override void Start()
    {
        base.Start();
    }
}
