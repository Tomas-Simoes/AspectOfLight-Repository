using UnityEngine;

[CreateAssetMenu(
    fileName = "newRangedAttackData",
    menuName = "Data/State Data/General Enemy/Ranged Attack State"
)]
public class D_EnemyRangedAttackState : ScriptableObject
{
    public float minimumDistanceToAttack;
    public LayerMask attackableLayer;
}
