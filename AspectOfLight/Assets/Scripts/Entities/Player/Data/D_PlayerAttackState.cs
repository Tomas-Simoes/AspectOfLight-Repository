using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAttackData", menuName = "Data/State Data/Player/Attack State")]
public class D_PlayerAttackState : ScriptableObject
{
    [Header("Attack")]
    public float maxAttackTime = 4;
    public float defaultDamage;
    public float changeDamageTimeJump = 1;
    public float damageIncrement;

    [Header("Stamina")]
    public float staminaCostTime;
    public int staminaCost;
}
