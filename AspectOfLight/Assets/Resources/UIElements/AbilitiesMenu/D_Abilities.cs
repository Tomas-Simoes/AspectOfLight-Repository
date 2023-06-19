using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(
    fileName = "New All Abilities",
    menuName = "Data/Ability/All Abilities Object",
    order = 0
)]
public class D_Abilities : ScriptableObject
{
    [SerializeReference]
    public List<Ability> abilities = new List<Ability>();
    public int selectedClassIndex;

    public GameObject abilityPrefab;
}
