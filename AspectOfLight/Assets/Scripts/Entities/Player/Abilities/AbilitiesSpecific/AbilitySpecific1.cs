using UnityEngine;

public class AbilitySpecific1 : Ability
{
    public AbilitySpecific1(float abilityCD, D_Ability abilityData)
        : base(abilityCD, abilityData) { }

    public override void Cast()
    {
        base.Cast();
        Debug.Log("Cast specific " + abilityData.abilityName);
    }
}
