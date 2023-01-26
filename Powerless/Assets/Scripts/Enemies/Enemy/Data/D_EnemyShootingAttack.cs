using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMoveData", menuName = "Data/State Data/Enemy/Shooting State")]
public class D_EnemyShootingAttack : ScriptableObject
{
    public float minimumDistanceToAttack;
    public LayerMask attackableLayer;

    public GameObject bullet;

    public float bulletSpeed;
    public float fireRate = 0.5f;
    private float nextFireTime;
}
