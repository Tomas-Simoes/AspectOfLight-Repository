using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : Ability
{
    [SerializeField]
    protected D_DashAbility abilityData;
    private float timeDashing = 0;

    public override void Cast(Player player)
    {
        base.Cast(player);

        ChangeAbilityInDedicatedState(true);

        this.player = player;

        timeDashing = 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        timeDashing += Time.deltaTime;

        if (timeDashing >= abilityData.dashTime)
        {
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

        Vector2 direction =
            player.axisMovement == Vector2.zero
                ? new Vector2(player.facingDirection, 0)
                : player.axisMovement.normalized;

        player.rb.velocity = direction * abilityData.dashSpeed;
    }
}
