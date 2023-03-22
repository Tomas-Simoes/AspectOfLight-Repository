using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySpecific2 : Ability
{
    public AbilitySpecific2(Player player, D_BaseAbility baseAbilityData)
        : base(player, baseAbilityData) { }

    public override void Cast()
    {
        base.Cast();
        Debug.Log("Cast specific ability two");
    }
}
