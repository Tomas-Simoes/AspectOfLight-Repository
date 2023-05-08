using UnityEngine;

[System.Serializable]
public class MagicSeedAbility : Ability
{
    [SerializeField]
    private D_MagicSeedAbility abilityData;
    private MagicSeedBullet magicSeedComponent;

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
