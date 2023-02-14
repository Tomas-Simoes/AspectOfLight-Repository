using UnityEngine;

[CreateAssetMenu(
    fileName = "newRangedAttackData",
    menuName = "Data/State Data/Enemy/Ranged Attack State"
)]
public class D_EnemyRangedAttackState : ScriptableObject
{
    public float minimumDistanceToAttack;
    public LayerMask attackableLayer;
    public float damage;
}
