using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySpecific2 : Ability
{
    public override void Cast(Player player)
    {
        base.Cast(player);
        Debug.Log("Cast specific ability two");
    }
}
