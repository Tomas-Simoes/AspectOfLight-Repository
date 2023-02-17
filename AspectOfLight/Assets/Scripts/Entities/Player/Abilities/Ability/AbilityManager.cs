using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager
{
    public Dictionary<int, Ability> playerAbilities = new Dictionary<int, Ability>();

    /*
        * Player Abilities       key            value
        *                         1      SpecificAbility1
        *                         2      SpecificAbility2
        *                         3          ...
    */

    public Dictionary<string, Ability> allAbilities;

    /*
        * All Abilities       key            value
        *                   "Drone"      SpecificAbility1
        *                   "Dash"       SpecificAbility2
        *                   "..."        SpecificAbility3
    */

    public AbilitySpecific1 specific1 { get; private set; }
    public AbilitySpecific2 specific2 { get; private set; }

    public void InitAbilities()
    {
        specific1 = new AbilitySpecific1(1);
        specific2 = new AbilitySpecific2(2);

        allAbilities = new Dictionary<string, Ability>()
        {
            { "Specific1", specific1 },
            { "Specific2", specific2 }
        };
    }

    public void AddPlayerAbility(int abilitySlot, string abilityName)
    {
        playerAbilities.Add(abilitySlot, allAbilities[abilityName]);
    }

    public void RemovePlayerAbility(int abilitySlot)
    {
        playerAbilities.Remove(abilitySlot);
    }

    public void CastAbility(int abilitySlot)
    {
        playerAbilities[abilitySlot].Cast();
    }
}
