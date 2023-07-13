using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "newMoveData",
    menuName = "Data/State Data/Player/Abilities/The Plague/Chase State"
)]
public class D_PlagueChaseState : ScriptableObject
{
    public float movementSpeed = 4;
    public float maxDistanceToChase = 10;
    public float minTimeToCheckClosestEnemy = 0.5f;
    public float minDistanceToPlayer = 1f;
}
