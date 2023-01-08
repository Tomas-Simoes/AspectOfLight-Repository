using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : StateRunner<PlayerController>, IDataPersistence
{
    public PlayerAnimation playerAnimation;

    protected override void Awake()
    {
        base.Awake();
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = this.transform.position;
    }
}
