using UnityEngine;

[System.Serializable]
public class MagicSeedAbility : Ability
{
    [SerializeField]
    private D_MagicCrystalAbility abilityData;

    public override void Cast(Player player)
    {
        base.Cast(player);

        GameObject _bullet = player.CreatePrefab(
            abilityData.bulletPrefab,
            player.attackPosition.transform.position,
            player.abilityInstances
        );

        _bullet.GetComponent<MagicCrystalBullet>().player = player;

        ResetCooldown();
        StopCastingAbility();
    }
}
