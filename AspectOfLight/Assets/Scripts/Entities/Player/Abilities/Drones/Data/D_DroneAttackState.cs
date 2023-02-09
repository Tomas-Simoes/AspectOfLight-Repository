using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMoveData", menuName = "Data/State Data/Drone/Attack State")]
public class D_DroneAttackState : ScriptableObject
{
    public float minimumDistanceToAttack = 1;
    public float damage = 4;
    public float explosionRange = 1;
}
