using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerNecromancerState : State
{
    public Player player;

    private D_PlayerNecromancerState stateData;
    private D_PlayerCoolDown coolDownData;

    public NecromancerMinium MiniumM;


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
        coolDownData.timSinceNecromancer = 0;
        Debug.Log("Enter the necromancer state");

        foreach (GameObject spawnpoint in player.dronesSpawnPoints)
        {
            Debug.Log("Create drone");
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

    public override void UpdateCooldowns()
    {
        base.UpdateCooldowns();
        coolDownData.timSinceNecromancer += Time.deltaTime;
    }
}
