using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : Ability
{
    [SerializeField]
    protected D_DashAbility abilityData;
    private float timeDashing = 0;
    private bool dashing = true;

    public override void Cast(Player player)
    {
        base.Cast(player);

        ChangeAbilityInDedicatedState(true);

        this.player = player;

        dashing = true;
        timeDashing = 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        timeDashing += Time.deltaTime;

        if (timeDashing >= abilityData.dashTime)
        {
            Debug.Log("Stopped Dashing");
            timeDashing = 0;
            player.timeSinceDash = 0;

            ChangeAbilityInDedicatedState(false);
            StopCastingAbility();
            ResetCooldown();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Debug.Log(player.axisMovement.normalized);
        player.rb.velocity = player.axisMovement.normalized * abilityData.dashSpeed;
    }
}
