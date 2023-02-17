using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySpecific2 : Ability
{
    public AbilitySpecific2(float abilityCD)
        : base(abilityCD) { }

    public override void Cast()
    {
        base.Cast();
        Debug.Log("Cast specific ability two");
    }
}
