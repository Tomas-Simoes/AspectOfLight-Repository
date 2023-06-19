using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class RadiantCrystalAbility : Ability
{
    [SerializeField]
    private D_RadiantCrystalAbility abilityData;
    private RadiantCrystal radiantCrystal = null;

    public override void Cast(Player player)
    {
        base.Cast(player);

        ChangeAbilityInDedicatedState(radiantCrystal == null, this, "laserAbility");

        if (radiantCrystal == null)
        {
            radiantCrystal = player
                .CreatePrefab(
                    abilityData.laserPrefab,
                    player.attackPosition.transform.position,
                    player.abilityInstances
                )
                .GetComponent<RadiantCrystal>();

            radiantCrystal.player = player;
        }
        else
        {
            ResetCooldown();
            GameObject.Destroy(radiantCrystal.gameObject);
        }
    }
}
