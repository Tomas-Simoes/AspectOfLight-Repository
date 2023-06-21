using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [Header("Player Information")]
    public float currentHealth;
    public int currentStamina;
    public int currentMiracles;

    [Header("Abilities Information")]
    public Dictionary<string, bool> abilityLockState;
    public Dictionary<int, string> playerAbilities;

    [Header("Tutorials Information")]
    public Dictionary<string, bool> tutorials;

    [Header("Bosses Information")]
    public Dictionary<string, bool> bosses;

    public long lastUpdated;
    public float movementSpeed;
    public int deathCount;
    public string areaInMap;
    public Vector2 playerPosition;

    public GameData()
    {
        this.currentStamina = 75;
        this.currentHealth = 45;
        this.currentMiracles = 1;

        this.playerPosition = new Vector2(0f, 0f);
        this.areaInMap = "ExiledFlorest";
        this.deathCount = 0;
        this.movementSpeed = 2;

        this.abilityLockState = new Dictionary<string, bool>();
        this.playerAbilities = new Dictionary<int, string>();

        this.tutorials = new Dictionary<string, bool>()
        {
            { "move", false },
            { "hurt", false },
            { "sigil", false }
        };

        this.bosses = new Dictionary<string, bool>() { { "thornborne", false } };
    }
}
