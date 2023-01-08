using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDataPersistence
{
    [SerializeField]
    public int currentHealth;

    [SerializeField]
    public int MAX_HEALTH;

    void Start()
    {
        currentHealth = MAX_HEALTH;
    }

    public void LoadData(GameData data)
    {
        this.currentHealth = data.currentHealth;
    }

    public void SaveData(GameData data)
    {
        data.currentHealth = this.currentHealth;
    }

    public void Damage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth < 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);

        //TODO Respawn in last spawnpoint
    }
}
