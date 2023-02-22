using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityManager : Menu
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

    [Header("Ability Slots")]
    [SerializeField]
    private GameObject firstSlot;

    [SerializeField]
    private GameObject secondSlot;

    [SerializeField]
    private GameObject thirdSlot;

    [Header("Others")]
    private bool abilityListAlreadyHasData;

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
            { magicSeedData.abilityName, magicSeed },
            { abilityTwoData.abilityName, specific2 }
        };
    }

    public void AddPlayerAbility(int abilitySlot, string abilityName)
    {
        if (playerAbilities.ContainsKey(abilitySlot))
            playerAbilities[abilitySlot] = allAbilities[abilityName];
        else
            playerAbilities.Add(abilitySlot, allAbilities[abilityName]);

        foreach (Ability ability in playerAbilities.Values)
        {
            Debug.Log(ability.abilityData.abilityName);
        }

        GameObject _slotToUpdate;
        switch (abilitySlot)
        {
            case 1:
                _slotToUpdate = firstSlot;
                break;
            case 2:
                _slotToUpdate = secondSlot;
                break;
            case 3:
                _slotToUpdate = thirdSlot;
                break;
            default:
                _slotToUpdate = null;
                break;
        }

        UpdateAbilityObject(_slotToUpdate, playerAbilities[abilitySlot].abilityData.abilityName);
    }

    public void RemovePlayerAbility(int abilitySlot)
    {
        playerAbilities.Remove(abilitySlot);
    }

    public void CastAbility(int abilitySlot)
    {
        playerAbilities[abilitySlot].Cast();
    }

    public void ListAbilities()
    {
        if (!abilityListAlreadyHasData)
        {
            foreach (Ability ability in allAbilities.Values)
            {
                GameObject _newAbilityObj = Instantiate(abilityPrefab);
                UpdateAbilityObject(_newAbilityObj, ability.abilityData.abilityName);

                _newAbilityObj.transform.SetParent(abilitiesParent.transform);
                _newAbilityObj.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

                _newAbilityObj
                    .GetComponent<Button>()
                    .onClick.AddListener(
                        delegate
                        {
                            AddPlayerAbility(1, ability.abilityData.abilityName);
                        }
                    );
            }
        }
    }

    private void UpdatePlayerAbilitySlots()
    {
        UpdateAbilityObject(firstSlot, playerAbilities[1].abilityData.abilityName);
        UpdateAbilityObject(secondSlot, playerAbilities[2].abilityData.abilityName);
        UpdateAbilityObject(thirdSlot, playerAbilities[3].abilityData.abilityName);
    }

    private void UpdateAbilityObject(GameObject abilityObj, string text)
    {
        abilityObj.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = text;
    }

    //? JUST FOR TESTS
    public void StopBackground()
    {
        Time.timeScale = 0f;
    }

    public void ResumeBackground()
    {
        Time.timeScale = 1f;
    }
}
