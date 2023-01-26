using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public PlayerMoveState moveState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerNecromancerState necromancerState { get; private set; }

    public InputHandler inputHandler { get; private set; }

    [Header("States Data")]
    [SerializeField]
    private D_PlayerCoolDown coolDownData;

    [SerializeField]
    private D_PlayerMoveState moveStateData;

    [SerializeField]
    private D_PlayerDashState dashStateData;

    [SerializeField]
    private D_PlayerNecromancerState necromancerStateData;

    [Header("Other Data")]
    public GameObject[] dronesSpawnPoints;

    public override void Start()
    {
        base.Start();
        currentHealth = entityData.MAX_HEALTH;

        if (inputHandler == null)
        {
            inputHandler = this.GetComponent<InputHandler>();
        }

        moveState = new PlayerMoveState(
            this,
            stateMachine,
            "move",
            moveStateData,
            this,
            coolDownData
        );
        dashState = new PlayerDashState(
            this,
            stateMachine,
            "dash",
            dashStateData,
            this,
            coolDownData
        );
        necromancerState = new PlayerNecromancerState(
            this,
            stateMachine,
            "necromancer",
            necromancerStateData,
            this,
            coolDownData
        );
        stateMachine.Init(moveState);
    }

    public virtual void CreatePrefab(
        GameObject prefabToSpawn,
        GameObject spawnPoint,
        GameObject parent
    )
    {
        var _newGameObject = Instantiate(
            prefabToSpawn,
            spawnPoint.transform.position,
            Quaternion.identity
        );
        _newGameObject.transform.parent = parent.transform;
    }
}
