using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyData", menuName = "Data/State Data/Enemy/Enemy Data")]
public class D_EnemyData : ScriptableObject
{
  public int MAX_HEALTH = 5;
  public GameObject target;
}
