using UnityEngine;

public class Ability
{
    public D_Ability abilityData;
    protected float abilityCD;

    public Ability(float abilityCD, D_Ability abilityData)
    {
        this.abilityCD = abilityCD;
        this.abilityData = abilityData;
    }

    public virtual void Cast() { }
}
