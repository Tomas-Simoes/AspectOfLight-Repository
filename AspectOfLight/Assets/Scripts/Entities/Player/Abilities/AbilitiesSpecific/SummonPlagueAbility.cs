using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPlagueAbility : Ability
{
    D_SummonPlagueAbility abilityData;

    public SummonPlagueAbility(
        Player player,
        D_BaseAbility baseAbilityData,
        D_SummonPlagueAbility abilityData
    )
        : base(player, baseAbilityData)
    {
        this.abilityData = abilityData;
    }

    public override void Cast()
    {
        base.Cast();

        foreach (GameObject spawnpoint in player.dronesSpawnPoints)
        {
            player.CreatePrefab(
                abilityData.plaguePrefab,
                spawnpoint.transform.position,
                GameObject.Find("AbilityInstances")
            );
        }
    }
}
