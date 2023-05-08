using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

  void Start()
  {
    InitAbilities();
  }

  protected override void OnEnable()
  {
    base.OnEnable();

    EventManager.StartListening("onSelectAbility", OnSelectAbility);
    EventManager.StartListening("onDeselectAbility", OnDeselectAbility);
  }

  private void OnDisable()
  {
    EventManager.StopListening("onSelectAbility", OnSelectAbility);
    EventManager.StopListening("onDeselectAbility", OnDeselectAbility);
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

      _newAbility.GetComponent<Button>().interactable = ability.isAvailable;

      _newAbility
          .GetComponent<Button>()
          .onClick.AddListener(
              delegate
              {
                PressedAvailableAbility(ability);
              }
          );

      _newAbility.GetComponent<ButtonSelected>().ability = ability;
    }
  }

  public void PressedAvailableAbility(Ability ability)
  {
    if (isChangingPlayerAbility)
    {
      if (AbilityManager.playerAbilities.ContainsValue(ability))
      {
        Debug.LogWarning("Ability already equiped");
        return;
      }

      if (AbilityManager.playerAbilities.ContainsKey(slotToChange))
        AbilityManager.playerAbilities[slotToChange] = ability;
      else
        AbilityManager.playerAbilities.Add(slotToChange, ability);

      Image _slotToUpdate = abilitySlots[slotToChange].GetComponent<Image>();

      _slotToUpdate.GetComponent<ButtonSelected>().ability = AbilityManager.playerAbilities[slotToChange];
      _slotToUpdate.GetComponent<Image>().sprite = AbilityManager.playerAbilities[slotToChange].iconAvailable;

      ResetUI();
    }
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
  public void ResetUI()
  {
    ChangeInteractablePlayerAbilitySlots(true, 0);
    abilitySlots[slotToChange].Select();

    ChangeRemovePlayerAbility(slotToChange, false);

    isChangingPlayerAbility = false;
  }
  public void OnSelectAbility(Dictionary<string, object> message)
  {
    Debug.Log("on select");

    descriptionAbilitySelected.SetActive(true);
    descriptionAbilityNotSelected.SetActive(false);

    descriptionIcon.sprite = (Sprite)message["icon"];
    descriptionName.text = (string)message["name"];
    descriptionText.text = (string)message["text"];
  }

  public void OnDeselectAbility(Dictionary<string, object> message)
  {
    Debug.Log("on deselect");
    descriptionAbilitySelected.SetActive(false);
    descriptionAbilityNotSelected.SetActive(true);
  }
}
