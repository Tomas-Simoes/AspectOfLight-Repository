using UnityEngine;

public class Ability
{
    public D_BaseAbility baseAbilityData;
    protected Player player;

    public Ability(Player player, D_BaseAbility baseAbilityData)
    {
        this.player = player;
        this.baseAbilityData = baseAbilityData;
    }

    public virtual void Cast() { }
}
