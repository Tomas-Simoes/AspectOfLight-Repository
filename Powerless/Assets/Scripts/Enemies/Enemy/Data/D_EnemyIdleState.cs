using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newIdleData", menuName = "Data/State Data/Enemy/Idle State")]
public class D_EnemyIdleState : ScriptableObject
{
    public float minIdleTime = 1f;
    public float maxIdleTime = 2f;
}
