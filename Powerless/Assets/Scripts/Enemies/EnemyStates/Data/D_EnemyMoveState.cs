using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMoveData", menuName = "Data/State Data/Enemy/Move State")]
public class D_EnemyMoveState : ScriptableObject
{
  public bool goTowardsOtherPatrolPoint = false;
  public float movementSpeed = 3;
}
