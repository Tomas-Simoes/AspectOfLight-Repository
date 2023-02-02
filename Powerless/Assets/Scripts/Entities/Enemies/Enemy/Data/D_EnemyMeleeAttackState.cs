using UnityEngine;

[CreateAssetMenu(fileName = "newAttackData", menuName = "Data/State Data/Enemy/Melee Attack State")]
public class D_EnemyMeleeAttackState : ScriptableObject
{
    public float minimumDistanceToAttack;
    public float attackRadius;
    public LayerMask attackableLayer;
    public float damage;
}
