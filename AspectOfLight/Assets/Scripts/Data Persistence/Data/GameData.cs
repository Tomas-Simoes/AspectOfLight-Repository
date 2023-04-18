using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public int currentHealth;
    public int deathCount;
    public string areaInMap;
    public Vector3 playerPosition;

    public GameData()
    {
        this.currentHealth = 10;
        this.playerPosition = new Vector3(0f, 0f, 0f);
        this.areaInMap = "ExiledFlorest";
        this.deathCount = 0;
    }
}
