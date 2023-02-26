using UnityEngine;

public class MagicSeedAbility : Ability
{
    D_MagicSeedAbility abilityData;

    public MagicSeedAbility(
        Player player,
        D_BaseAbility baseAbilityData,
        D_MagicSeedAbility abilityData
    )
        : base(player, baseAbilityData)
    {
        this.abilityData = abilityData;
    }

    public override void Cast()
    {
        base.Cast();
        Debug.Log("Cast magic seed");
    }
}
