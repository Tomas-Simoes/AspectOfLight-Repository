using UnityEngine;

[CreateAssetMenu(
    fileName = "newRecoveryAttackData",
    menuName = "Data/State Data/General Enemy/Recovery Attack State"
)]
public class D_EnemyRecoveryAttackState : ScriptableObject
{
    public float recoveryAreaRadius;
    public float recoveryMovementSpeed;
}
