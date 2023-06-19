using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Ability
{
    public string abilityName = "No name";
    public string abilityPath;
    public float abilityCD = 0;
    public int staminaCost = 0;
    public int unlockCost = 0;
    public bool isAvailable = false;
    public Sprite iconAvailable;
    public Sprite iconUnavailable;
    public bool canBeRecasted;

    [TextArea(3, 10)]
    public string description = "No description yet";

    protected Player player;

    public virtual void Cast(Player player) { }

    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate() { }

    protected void StopCastingAbility()
    {
        EventManager.TriggerEvent(
            "onStopAbilityCast",
            false,
            new Dictionary<string, object> { { "abilityName", abilityName } }
        );
    }

    public virtual void ResetCooldown()
    {
        EventManager.TriggerEvent(
            "onAbilityResetCooldown",
            false,
            new Dictionary<string, object> { { "abilityName", abilityName } }
        );
    }

    public virtual void ChangeAbilityInDedicatedState(
        bool activeCondition,
        Ability ability = null,
        string animatorName = ""
    )
    {
        EventManager.TriggerEvent(
            "onPlayerCastingState",
            false,
            new Dictionary<string, object>
            {
                { "activeState", activeCondition },
                { "ability", ability },
                { "animName", animatorName }
            }
        );
    }
}
