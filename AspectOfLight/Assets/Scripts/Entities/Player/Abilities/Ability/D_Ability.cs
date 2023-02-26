using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/Create New Ability")]
public class D_Ability : ScriptableObject
{
    public string abilityName = "No name";
    public Sprite icon;
    public float abilityCD;
    public string description = "No description yet";
}
