using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMoveData", menuName = "Data/State Data/Player/Abilities/The Plague/Attack State")]
public class D_PlagueAttackState : ScriptableObject
{
    public float minimumDistanceToAttack = 1;
    public float damage = 4;
    public float explosionRange = 1;
}
