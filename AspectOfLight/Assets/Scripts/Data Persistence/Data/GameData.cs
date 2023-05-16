using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [Header("Player Information")]
    public float currentHealth;
    public int currentStamina;

    [Header("Abilities Information")]
    public Dictionary<string, bool> abilityLockState;

    public long lastUpdated;
    public float movementSpeed;
    public int deathCount;
    public string areaInMap;
    public Vector3 playerPosition;

    public GameData()
    {
        Debug.Log("pov");

        this.currentStamina = 35;
        this.currentHealth = 45;
        this.playerPosition = new Vector3(0f, 0f, 0f);
        this.areaInMap = "ExiledFlorest";
        this.deathCount = 0;
        this.movementSpeed = 2;
        this.abilityLockState = new Dictionary<string, bool>();
    }
}
