using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMoveData", menuName = "Data/State Data/Enemy/Shooting State")]
public class D_EnemyShootingAttack : ScriptableObject
{
    public float minimumDistanceToAttack;
    public float attackRadius;
    public LayerMask attackableLayer;
}
