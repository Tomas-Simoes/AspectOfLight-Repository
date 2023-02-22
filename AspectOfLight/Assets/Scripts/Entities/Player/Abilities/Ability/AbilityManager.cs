using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityManager : MonoBehaviour
{
    [Header("Ability Data")]
    [SerializeField]
    private D_Ability magicSeedData;

    [SerializeField]
    private D_Ability abilityTwoData;

    public Dictionary<int, Ability> playerAbilities = new Dictionary<int, Ability>();

    /*
        * Player Abilities       key            value
        *                         1      SpecificAbility1
        *                         2      SpecificAbility2
        *                         3          ...
    */

    public Dictionary<string, Ability> allAbilities;

    /*
        * All Abilities       key            value
        *                   "Drone"      SpecificAbility1
        *                   "Dash"       SpecificAbility2
        *                   "..."        SpecificAbility3
    */

    [Header("Others")]
    [SerializeField]
    private GameObject abilityPrefab;

    [SerializeField]
    private GameObject abilitiesParent;

    public Ability magicSeed { get; private set; }
    public Ability specific2 { get; private set; }

    public void InitAbilities()
    {
        magicSeed = new AbilitySpecific1(1, magicSeedData);
        specific2 = new AbilitySpecific2(2, abilityTwoData);

        allAbilities = new Dictionary<string, Ability>()
        {
            { "MagicSeed", magicSeed },
            { "Specific2", specific2 }
        };
    }

    public void AddPlayerAbility(int abilitySlot, string abilityName)
    {
        playerAbilities.Add(abilitySlot, allAbilities[abilityName]);
    }

    public void RemovePlayerAbility(int abilitySlot)
    {
        playerAbilities.Remove(abilitySlot);
    }

    public void CastAbility(int abilitySlot)
    {
        playerAbilities[abilitySlot].Cast();
    }

    public void ListItems()
    {
        foreach (Ability ability in allAbilities.Values)
        {
            var newAbilityObj = Instantiate(abilityPrefab);

            newAbilityObj.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = ability
                .abilityData
                .abilityName;

            newAbilityObj.transform.SetParent(abilitiesParent.transform);
            newAbilityObj.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
