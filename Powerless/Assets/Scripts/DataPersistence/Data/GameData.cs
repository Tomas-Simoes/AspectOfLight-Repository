using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    Health health;
    public int currentHealth;

    void Start()
    {
        if (health == null)
            GameObject.Find("Player").GetComponent<Health>();
    }

    public GameData()
    {
        this.currentHealth = 10;
    }
}
