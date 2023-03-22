using UnityEngine;

public class MagicSeedAbility : Ability
{
    private D_MagicSeedAbility abilityData;
    private MagicSeedBullet magicSeedComponent;

    public MagicSeedAbility(
        Player player,
        D_BaseAbility baseAbilityData,
        D_MagicSeedAbility abilityData
    )
        : base(player, baseAbilityData)
    {
        this.abilityData = abilityData;
    }

    public override void Cast()
    {
        base.Cast();

        GameObject _bullet = player.CreatePrefab(
            abilityData.bulletPrefab,
            player.attackPosition.transform.position,
            player.bulletsParent
        );

        magicSeedComponent = _bullet.GetComponent<MagicSeedBullet>();
        magicSeedComponent.player = player;
    }
}
