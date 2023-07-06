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
    private List<Button> playerAbilitySlots;

    private List<Button> availableAbilitySlots = new List<Button>();

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

    [Header("On Prompt Callbacks")]
    UIManager.OnPromptYes OnYesUnlockingNewAbility;
    UIManager.OnPromptNo OnNoUnlockingNewAbility;

    [Header("Others")]
    [SerializeField]
    private Sprite emptySlotSprite;

    void Start()
    {
        InitAbilities();

        OnYesUnlockingNewAbility = (message) =>
        {
            UnlockPlayerAbility((Image)message["image"], (Ability)message["ability"]);
            isChangingPlayerAbility = (bool)message["isChangingAbility"];

            if (!isChangingPlayerAbility)
            {
                ((Image)message["image"]).gameObject.GetComponent<Button>().Select();
            }
            else
            {
                OnAvailableAbilityPressed(
                    (Image)message["image"],
                    (Ability)message["ability"],
                    false
                );
            }

            Destroy((GameObject)message["prompt"]);
            ChangeInteractableAvailableAbilities(true);
        };

        OnNoUnlockingNewAbility = (message) =>
        {
            Destroy((GameObject)message["prompt"]);

            ResetUIAfterChangingPlayerAbility();
            ChangeInteractableAvailableAbilities(true);
        };

        miraclesText.text = AbilityManager.Instance.currentMiracles.ToString();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        UpdateAbilitySlots();

        EventManager.StartListening("onSelectAbility", OnSelectAbility);
        EventManager.StartListening("onDeselectAbility", OnDeselectAbility);
        EventManager.StartListening("onExitInAbilityTree", OnExitInAbilityTree);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onSelectAbility", OnSelectAbility);
        EventManager.StopListening("onDeselectAbility", OnDeselectAbility);
        EventManager.StopListening("onExitInAbilityTree", OnExitInAbilityTree);
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
                        OnAvailableAbilityPressed(_newAbility.GetComponent<Image>(), ability);
                    }
                );

            _newAbility.GetComponent<AbilitySelected>().ability = ability;

            availableAbilitySlots.Add(_newAbility.GetComponent<Button>());
        }
    }

    public void OnAvailableAbilityPressed(Image image, Ability ability, bool promptPlayer = true)
    {
        //TODO show some history context
        if (ability.isAvailable && !isChangingPlayerAbility)
        {
            return;
        }

        //TODO make this warning logs for player
        if (!ability.isAvailable && !AbilityManager.Instance.CanUnlockAbility(ability))
        {
            Debug.LogWarning("No miracles");
            return;
        }

        if (isChangingPlayerAbility)
        {
            //TODO make this warning logs for player
            if (AbilityManager.playerAbilities.ContainsValue(ability))
            {
                Debug.LogWarning("Ability already equiped");

                ResetUIAfterChangingPlayerAbility();
                return;
            }

            if (!ability.isAvailable)
            {
                ChangeInteractableAvailableAbilities(false);
                OpenUnlockingAbilityPrompt(image, ability, true);

                return;
            }

            if (AbilityManager.playerAbilities.ContainsKey(slotToChange))
                AbilityManager.playerAbilities[slotToChange] = ability;
            else
                AbilityManager.playerAbilities.Add(slotToChange, ability);

            AbilityManager.Instance.ForcebilityCooldown(
                AbilityManager.playerAbilities[slotToChange].abilityName
            );

            Image _slotToUpdate = playerAbilitySlots[slotToChange].GetComponent<Image>();

            _slotToUpdate.GetComponent<AbilitySelected>().ability = AbilityManager.playerAbilities[
                slotToChange
            ];
            _slotToUpdate.sprite = AbilityManager.playerAbilities[slotToChange].iconAvailable;

            ResetUIAfterChangingPlayerAbility();
            return;
        }

        if (promptPlayer)
        {
            OpenUnlockingAbilityPrompt(image, ability, false);
            return;
        }
    }

    public void OnPlayerAbilityPressed(int slot)
    {
        //TODO make this warning logs for player
        if (AbilityManager.playerAbilities.ContainsKey(slot))
        {
            Debug.LogWarning("Tried to change already occupied slot");
            return;
        }

        ChangeInteractablePlayerAbility(false, slot);

        isChangingPlayerAbility = true;
        slotToChange = slot;

        if (AbilityManager.playerAbilities.ContainsKey(slot))
            ChangeRemovePlayerAbility(slot, true);
    }

    public void UnlockPlayerAbility(Image image, Ability ability)
    {
        if (AbilityManager.Instance.UnlockAbility(ability))
        {
            image.sprite = ability.iconAvailable;
            miraclesText.text = AbilityManager.Instance.currentMiracles.ToString();
        }
    }

    public void OnExitPressed()
    {
        ResetUIAfterChangingPlayerAbility();

        EventManager.TriggerEvent(
            "onAbilityTree",
            true,
            new Dictionary<string, object> { { "activeState", false } }
        );
    }

    private void ChangeInteractablePlayerAbility(bool state, int playerAbilitySlotPressed)
    {
        foreach (Button slot in playerAbilitySlots)
        {
            slot.interactable = state;
        }

        if (!state)
        {
            playerAbilitySlots[playerAbilitySlotPressed].Select();
            playerAbilitySlots[playerAbilitySlotPressed].interactable = true;
        }
    }

    private void ChangeInteractableAvailableAbilities(bool state)
    {
        foreach (Button slot in availableAbilitySlots)
        {
            slot.interactable = state;
        }
    }

    //TODO this
    private void ChangeRemovePlayerAbility(int slot, bool state)
    {
        /*
         for (int i = 0; i <= 4; i++)
        {
            ButtonSelected _playerAbilitySlotButton = playerAbilitySlots[
                i
            ].GetComponent<ButtonSelected>();

            if (_playerAbilitySlotButton.isSelected)
            {
                _playerAbilitySlotButton.ability = null;
                playerAbilitySlots[i].GetComponent<Image>().sprite = emptySlotSprite;

                AbilityManager.Instance.RemovePlayerAbility(i);
                ResetUIAfterChangingPlayerAbility();

                return;
            }
        }
        
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

    private void OnExitInAbilityTree(Dictionary<string, object> message)
    {
        if (isChangingPlayerAbility)
        {
            ResetUIAfterChangingPlayerAbility();
            return;
        }

        EventManager.TriggerEvent(
            "onAbilityTree",
            true,
            new Dictionary<string, object> { { "activeState", false } }
        );
    }

    public void ResetUIAfterChangingPlayerAbility()
    {
        ChangeInteractablePlayerAbility(true, 0);
        playerAbilitySlots[slotToChange].Select();
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

    public void OpenUnlockingAbilityPrompt(Image image, Ability ability, bool isChangingAbility)
    {
        EventManager.TriggerEvent(
            "onPrompt",
            true,
            new Dictionary<string, object>
            {
                {
                    "promptText",
                    "Are you sure that you want this abiity? \n It will cost you "
                        + ability.unlockCost
                        + (ability.unlockCost != 1 ? " miracles." : "miracle.")
                },
                { "yesCallback", OnYesUnlockingNewAbility },
                { "noCallback", OnNoUnlockingNewAbility },
                {
                    "callbackMessage",
                    new Dictionary<string, object>
                    {
                        { "image", image },
                        { "ability", ability },
                        { "isChangingAbility", isChangingAbility },
                        {
                            "prompt",
                            UIManager.Instance.LoadUIObject("UIElements/PromptUI/PromptUI")
                        }
                    }
                }
            }
        );
    }

    public void UpdateAbilitySlots()
    {
        foreach (KeyValuePair<int, Ability> playerAbility in AbilityManager.playerAbilities)
        {
            GameObject currentSlot = playerAbilitySlots[playerAbility.Key].gameObject;

            currentSlot.GetComponent<Image>().sprite = playerAbility.Value.iconAvailable;
            currentSlot.GetComponent<AbilitySelected>().ability = playerAbility.Value;
        }
    }
}
