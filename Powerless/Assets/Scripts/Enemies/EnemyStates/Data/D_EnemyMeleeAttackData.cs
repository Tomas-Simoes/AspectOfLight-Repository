using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAttackData", menuName = "Data/State Data/Enemy/Melee Attack State")]
public class D_EnemyMeleeAttackData : ScriptableObject
{
    public float minimumDistanceToAttack;
    public float attackRadius;
    public LayerMask attackableLayer;
}
