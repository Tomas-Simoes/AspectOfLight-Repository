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
        playerPosition = Vector3.zero;
        this.currentHealth = 10;
    }
}
