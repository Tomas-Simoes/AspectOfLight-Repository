using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPlagueAbility : Ability
{
    [SerializeField]
    D_SummonPlagueAbility abilityData;

    public override void Cast()
    {
        base.Cast();

        foreach (GameObject spawnpoint in player.plagueSpawnPoints)
        {
            player.CreatePrefab(
                abilityData.plaguePrefab,
                spawnpoint.transform.position,
                GameObject.Find("AbilityInstances") //TODO have a reference to ability instances somewhere
            );
        }
    }
}
