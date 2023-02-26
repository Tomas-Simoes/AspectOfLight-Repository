using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/Create Base Data")]
public class D_BaseAbility : ScriptableObject
{
    public string abilityName = "No name";
    public Sprite icon;
    public float abilityCD;
    public string description = "No description yet";
}
