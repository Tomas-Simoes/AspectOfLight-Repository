using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : StateRunner<PlayerController>, IDataPersistence
{
    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }
}
