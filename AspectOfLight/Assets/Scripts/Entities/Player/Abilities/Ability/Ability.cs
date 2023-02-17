using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    protected float abilityCD;

    public Ability(float abilityCD)
    {
        this.abilityCD = abilityCD;
    }

    public virtual void Cast() { }
}
