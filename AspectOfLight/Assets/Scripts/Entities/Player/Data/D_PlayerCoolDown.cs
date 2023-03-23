using UnityEngine;

[CreateAssetMenu(fileName = "newCoolDownData", menuName = "Data/Cooldowns/Player Cooldown")]
public class D_PlayerCoolDown : ScriptableObject
{
    public float dashCD = 4;
    public float necromancerCD = 10;
    public float attackCD = 1;
}
