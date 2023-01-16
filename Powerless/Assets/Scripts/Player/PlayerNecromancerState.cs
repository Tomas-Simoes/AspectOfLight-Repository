using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerNecromancerState : State
{
    public Player player;

    private D_PlayerNecromancerState stateData;
    private D_PlayerCoolDown coolDownData;


    public PlayerNecromancerState( Entity entity, 
        StateMachine stateMachine, 
        string animBoolName, 
        D_PlayerNecromancerState stateData, 
        Player player,
        D_PlayerCoolDown coolDownData
  ) : base(entity, stateMachine, animBoolName)
    {
        this.player = player;
        this.stateData = stateData;
        this.coolDownData = coolDownData;
    }

    public override void Enter()
    {
        base.Enter();
        coolDownData.timeSinceNecromancer = 0;

        foreach (GameObject spawnpoint in player.dronesSpawnPoints)
        {
            Debug.Log("Create " + stateData.drone + " in " + spawnpoint);
            player.CreatePrefab(stateData.drone, spawnpoint, GameObject.Find("Drones"));    
        }

        stateMachine.ChangeState(player.moveState);
    }

    public override void Exit() 
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
