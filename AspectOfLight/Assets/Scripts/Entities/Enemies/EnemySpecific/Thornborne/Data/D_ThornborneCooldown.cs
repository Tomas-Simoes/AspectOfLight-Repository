using UnityEngine;

[CreateAssetMenu(
    fileName = "D_ThornborneCooldown",
    menuName = "Data/Cooldowns/Specific Enemy/Thornborne Cooldown",
    order = 0
)]
public class D_ThornborneCooldown : ScriptableObject
{
    public float ugAttackCD;
    public float ugTeleportCD;
}
