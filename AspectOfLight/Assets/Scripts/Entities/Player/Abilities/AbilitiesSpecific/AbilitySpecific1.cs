using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySpecific1 : Ability
{
    public AbilitySpecific1(float abilityCD)
        : base(abilityCD) { }

    public override void Cast()
    {
        base.Cast();
        Debug.Log("Cast specific ability one");
    }
}
