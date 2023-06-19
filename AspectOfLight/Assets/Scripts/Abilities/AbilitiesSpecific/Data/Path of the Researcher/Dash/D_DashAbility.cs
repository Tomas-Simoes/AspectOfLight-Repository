using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDashData", menuName = "Data/State Data/Player/Dash State")]
public class D_DashAbility : ScriptableObject
{
    public float dashSpeed = 6;
    public float dashTime = 2;
    public float staminaCost = 1;
}
