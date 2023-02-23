using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    [Header("UI Elements")]
    [SerializeField]
    private Button firstSlot;

    [SerializeField]
    private Button secondSlot;

    [SerializeField]
    private Button thirdSlot;

    [SerializeField]
    private Button closeButton;

    [SerializeField]
    private TextMeshProUGUI informationText;

    [SerializeField]
    private TextMeshProUGUI abilitiesCountText;

    [SerializeField]
    private TextMeshProUGUI abilityDescription;

    [Header("Ability Information")]
    [SerializeField]
    private GameObject abilityPrefab;

    [SerializeField]
    private GameObject abilitiesParent;

    public Ability magicSeed { get; private set; }
    public Ability specific2 { get; private set; }

    [Header("Change Ability Information")]
    private bool isChangingPlayerAbility;
    private int slotToChange;

    [Header("Others")]
    private bool abilityListAlreadyHasData;

    [SerializeField]
    InputHandler inputHandler;

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

    public void SelectPlayerAbility(string abilityName)
    {
        if (isChangingPlayerAbility)
        {
            if (playerAbilities.ContainsValue(allAbilities[abilityName]))
            {
                informationText.text = "This ability is already in a slot";
                return;
            }

            if (playerAbilities.ContainsKey(slotToChange))
                playerAbilities[slotToChange] = allAbilities[abilityName];
            else
                playerAbilities.Add(slotToChange, allAbilities[abilityName]);

            Button _slotToUpdate;
            switch (slotToChange)
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

            UpdateAbilityObject(
                _slotToUpdate.gameObject,
                playerAbilities[slotToChange].abilityData.abilityName
            );

            ChangeInteractablePlayerAbilitySlots(true);
            closeButton.interactable = true;

            isChangingPlayerAbility = false;
            _slotToUpdate.Select();

            informationText.text = "Abilities";
        }
        else { }
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

                _newAbilityObj.GetComponent<ButtonDescription>().description = ability
                    .abilityData
                    .description;

                _newAbilityObj.transform.SetParent(abilitiesParent.transform);
                _newAbilityObj.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

                _newAbilityObj
                    .GetComponent<Button>()
                    .onClick.AddListener(
                        delegate
                        {
                            SelectPlayerAbility(ability.abilityData.abilityName);
                        }
                    );
            }

            abilityListAlreadyHasData = true;
        }
    }

    public void UpdatePlayerAbilitySlot(int slot)
    {
        informationText.text = "Choose an ability";

        abilitiesParent.transform.GetChild(0).GetComponent<Button>().Select();

        ChangeInteractablePlayerAbilitySlots(false);
        closeButton.interactable = false;

        isChangingPlayerAbility = true;
        slotToChange = slot;
    }

    private void ChangeInteractablePlayerAbilitySlots(bool state)
    {
        firstSlot.interactable = state;
        secondSlot.interactable = state;
        thirdSlot.interactable = state;
    }

    private void UpdateAbilityObject(GameObject abilityObj, string text)
    {
        abilityObj.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = text;
    }

    public void ChangeDescription(string description)
    {
        abilityDescription.text = description;
        Debug.Log("pov" + abilityDescription.text);
    }

    void Update()
    {
        Debug.Log(abilityDescription.text);
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
