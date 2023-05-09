using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityMenu : Menu
{
    [Header("UI Description")]
    [SerializeField]
    public GameObject descriptionAbilitySelected;

    [SerializeField]
    public GameObject descriptionAbilityNotSelected;

    [SerializeField]
    public Image descriptionIcon;

    [SerializeField]
    public TextMeshProUGUI descriptionName;

    [SerializeField]
    public TextMeshProUGUI descriptionText;

    [Header("UI Slots")]
    [SerializeField]
    private List<Button> abilitySlots;

    //? index 0 > passive
    //? index 1 / 2 / 3 > ability slots 1 / 2 / 3

    [Header("Path Objects")]
    [SerializeField]
    private GameObject pathOfTheLuminary;

    [SerializeField]
    private GameObject pathOfTheMartial;

    [SerializeField]
    private GameObject pathOfTheUnnatural;

    [SerializeField]
    private GameObject pathOfTheResearcher;

    [SerializeField]
    private GameObject pathOfTheEngineer;

    [Header("Change Ability Information")]
    private bool isChangingPlayerAbility;
    private int slotToChange;

    [Header("Miracles")]
    [SerializeField]
    private TextMeshProUGUI miraclesText;

    [Header("Others")]
    [SerializeField]
    private Sprite emptySlotSprite;

    void Start()
    {
        InitAbilities();
        miraclesText.text = AbilityManager.Instance.playerMiracles.ToString();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        EventManager.StartListening("onSelectAbility", OnSelectAbility);
        EventManager.StartListening("onDeselectAbility", OnDeselectAbility);
        EventManager.StartListening("onCancel", OnCancel);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onSelectAbility", OnSelectAbility);
        EventManager.StopListening("onDeselectAbility", OnDeselectAbility);
        EventManager.StopListening("onCancel", OnCancel);
    }

    private void InitAbilities()
    {
        foreach (Ability ability in AbilityManager.Instance.abilitiesData.abilities)
        {
            GameObject _newAbility = Instantiate(
                AbilityManager.Instance.abilitiesData.abilityPrefab
            );

            switch (ability.abilityPath)
            {
                case "Path of the Luminary":
                    _newAbility.transform.SetParent(pathOfTheLuminary.transform, false);
                    break;
                case "Path of the Unnatural":
                    _newAbility.transform.SetParent(pathOfTheUnnatural.transform, false);
                    break;
                case "Path of the Researcher":
                    _newAbility.transform.SetParent(pathOfTheResearcher.transform, false);
                    break;
                case "Path of the Martial":
                    _newAbility.transform.SetParent(pathOfTheMartial.transform, false);
                    break;
                case "Path of the Engineer":
                    _newAbility.transform.SetParent(pathOfTheEngineer.transform, false);
                    break;
                default:
                    break;
            }

            _newAbility.GetComponent<Image>().sprite = ability.isAvailable
                ? ability.iconAvailable
                : ability.iconUnavailable;

            _newAbility
                .GetComponent<Button>()
                .onClick.AddListener(
                    delegate
                    {
                        PressedAvailableAbility(_newAbility.GetComponent<Image>(), ability);
                    }
                );

            _newAbility.GetComponent<ButtonSelected>().ability = ability;
        }
    }

    public void PressedAvailableAbility(Image image, Ability ability)
    {
        if (isChangingPlayerAbility)
        {
            //TODO make this warning logs for player
            if (AbilityManager.playerAbilities.ContainsValue(ability))
            {
                Debug.LogWarning("Ability already equiped");
                return;
            }

            if (!ability.isAvailable)
            {
                Debug.LogWarning("Ability is locked");
                return;
            }

            if (AbilityManager.playerAbilities.ContainsKey(slotToChange))
                AbilityManager.playerAbilities[slotToChange] = ability;
            else
                AbilityManager.playerAbilities.Add(slotToChange, ability);

            Image _slotToUpdate = abilitySlots[slotToChange].GetComponent<Image>();

            _slotToUpdate.GetComponent<ButtonSelected>().ability = AbilityManager.playerAbilities[
                slotToChange
            ];
            _slotToUpdate.GetComponent<Image>().sprite = AbilityManager.playerAbilities[
                slotToChange
            ].iconAvailable;

            ResetUI();
            return;
        }

        if (AbilityManager.Instance.UnlockAbility(ability))
        {
            image.sprite = ability.iconAvailable;
            miraclesText.text = AbilityManager.Instance.playerMiracles.ToString();
        }
    }

    public void OnExitPressed()
    {
        EventManager.TriggerEvent(
            "onAbilityTree",
            true,
            new Dictionary<string, object> { { "activeState", false } }
        );

        ResetUI();
    }

    public void OnPlayerAbilityPressed(int slot)
    {
        ChangeInteractablePlayerAbilitySlots(false, slot);

        isChangingPlayerAbility = true;
        slotToChange = slot;

        if (AbilityManager.playerAbilities.ContainsKey(slot))
            ChangeRemovePlayerAbility(slot, true);
    }

    private void ChangeInteractablePlayerAbilitySlots(bool state, int playerAbilitySlotPressed)
    {
        foreach (Button slot in abilitySlots)
            slot.interactable = state;

        if (!state)
        {
            abilitySlots[playerAbilitySlotPressed].Select();
            abilitySlots[playerAbilitySlotPressed].interactable = true;
        }
    }

    //TODO this
    private void ChangeRemovePlayerAbility(int slot, bool state)
    {
        /*
        switch (slot)
        {
          case 1:
            removeSlotOneButton.gameObject.SetActive(state);
            break;
          case 2:
            removeSlotTwoButton.gameObject.SetActive(state);
            break;
          case 3:
            removeSlotThreeButton.gameObject.SetActive(state);
            break;
          default:
            break;
        }
        */
    }

    private void OnCancel(Dictionary<string, object> message)
    {
        if (isChangingPlayerAbility)
        {
            ResetUI();
            return;
        }

        for (int i = 0; i <= 4; i++)
        {
            ButtonSelected _abilitySlotButton = abilitySlots[i].GetComponent<ButtonSelected>();

            if (_abilitySlotButton.isSelected)
            {
                _abilitySlotButton.ability = null;
                abilitySlots[i].GetComponent<Image>().sprite = emptySlotSprite;

                AbilityManager.Instance.RemovePlayerAbility(i);
                ResetUI();

                return;
            }
        }
    }

    public void ResetUI()
    {
        ChangeInteractablePlayerAbilitySlots(true, 0);
        abilitySlots[slotToChange].Select();
        isChangingPlayerAbility = false;
    }

    public void OnSelectAbility(Dictionary<string, object> message)
    {
        descriptionAbilitySelected.SetActive(true);
        descriptionAbilityNotSelected.SetActive(false);

        descriptionIcon.sprite = (Sprite)message["icon"];
        descriptionName.text = (string)message["name"];
        descriptionText.text = (string)message["text"];
    }

    public void OnDeselectAbility(Dictionary<string, object> message)
    {
        descriptionAbilitySelected.SetActive(false);
        descriptionAbilityNotSelected.SetActive(true);
    }
}
