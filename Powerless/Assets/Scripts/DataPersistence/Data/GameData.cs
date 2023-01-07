using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentHealth;
    public Vector3 playerPosition;

    public GameData()
    {
        this.currentHealth = 10;
        this.playerPosition = Vector3.zero;
    }
}
