using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThePlagueAbility : Ability
{
    [SerializeField]
    D_ThePlagueAbility abilityData;

    public override void Cast(Player player)
    {
        base.Cast(player);

        foreach (GameObject spawnpoint in player.plagueSpawnPoints)
        {
            player.CreatePrefab(
                abilityData.plaguePrefab,
                spawnpoint.transform.position,
                player.abilityInstances
            );

            ResetCooldown();
            StopCastingAbility();
        }
    }
}
