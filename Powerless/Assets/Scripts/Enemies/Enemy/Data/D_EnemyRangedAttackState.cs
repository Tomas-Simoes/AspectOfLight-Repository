using UnityEngine;

[CreateAssetMenu(
    fileName = "newRangedAttackData",
    menuName = "Data/State Data/Enemy/Ranged Attack State"
)]
public class D_EnemyRangedAttackState : ScriptableObject
{
    public float minimumDistanceToAttack;
    public LayerMask attackableLayer;

    public GameObject bullet;

    public float bulletSpeed;
    public float fireRate = 0.5f;
    private float nextFireTime;
}
