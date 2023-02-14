using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMoveData", menuName = "Data/State Data/Player/Shooting State")]
public class D_PlayerShootingState : ScriptableObject
{
    public GameObject bulletPrefab;

    public Transform barrel;

    public GameObject bulletsParent;

    public float damage;
}
