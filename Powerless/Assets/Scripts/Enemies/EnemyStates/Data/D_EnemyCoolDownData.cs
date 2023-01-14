using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCooldownData", menuName = "Data/State Data/Enemy/Cooldown")]
public class D_EnemyCoolDownData : ScriptableObject
{
    public float normalAttackCD = 5;
    public float timeSinceNormalAttack = 5;
}
