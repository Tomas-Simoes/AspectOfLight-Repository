using UnityEngine;

[System.Serializable]
public class MagicSeedAbility : Ability
{
    [SerializeField]
    private D_MagicSeedAbility abilityData;
    private MagicSeedBullet magicSeedComponent;

    public override void Cast(Player player)
    {
        base.Cast(player);

        GameObject _bullet = player.CreatePrefab(
            abilityData.bulletPrefab,
            player.attackPosition.transform.position,
            player.bulletsParent
        );

        magicSeedComponent = _bullet.GetComponent<MagicSeedBullet>();
        magicSeedComponent.player = player;
    }
}
