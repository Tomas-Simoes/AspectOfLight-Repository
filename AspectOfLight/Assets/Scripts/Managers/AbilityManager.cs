using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityManager : Singleton<AbilityManager>
{

  [Header("Abilities Information")]
  public D_Abilities abilitiesData;

  // public List<Ability> abilities = new List<Ability>();

  [Header("Player Abilities Information")]
  public static Dictionary<int, Ability> playerAbilities = new Dictionary<int, Ability>();

  [Header("Ability Information")]
  private float timeSinceAbilityOne;
  private float timeSinceAbilityTwo;
  private float timeSinceAbilityThree;

  private void Update()
  {
    UpdateCooldowns();

  }
  private void UpdateCooldowns()
  {
    timeSinceAbilityOne += Time.deltaTime;
    timeSinceAbilityTwo += Time.deltaTime;
    timeSinceAbilityThree += Time.deltaTime;
  }

  private void OnEnable()
  {
    EventManager.StartListening("onAbilityCast", CastAbility);
  }

  private void OnDisable()
  {
    EventManager.StopListening("onAbilityCast", CastAbility);

  }

  public void CastAbility(Dictionary<string, object> message)
  {
    int _currentSlot = (int)message["slot"];

    bool _hasStaminaToCast =
        (int)message["currentStamina"] - playerAbilities[_currentSlot].staminaCost >= 0;

    if (
        GetTimeSinceCast(_currentSlot) >= playerAbilities[_currentSlot].abilityCD
        && _hasStaminaToCast
    )
    {

      playerAbilities[_currentSlot].Cast();
    }
    else
    {
      //TODO play some visual indicator that user has no stamina;
    }
  }

  private float GetTimeSinceCast(int slot)
  {
    switch (slot)
    {
      case 1:
        return timeSinceAbilityOne;
      case 2:
        return timeSinceAbilityTwo;
      case 3:
        return timeSinceAbilityThree;
    }

    return 0;
  }

  /*
      public void ResetUI()
      {
          ChangeInteractablePlayerAbilitySlots(true);

          closeButton.interactable = true;
          cancelButton.gameObject.SetActive(false);
          closeButton.gameObject.SetActive(true);

          GetPlayerSlotButton(slotToChange).Select();
          ChangeRemovePlayerAbility(slotToChange, false);

          informationText.text = "Abilities";

          isChangingPlayerAbility = false;
      }
  */
  /*
  public Dictionary<int, Ability> playerAbilities = new Dictionary<int, Ability>();

      * Player Abilities       key            value
      *                         1      SpecificAbility1
      *                         2      SpecificAbility2
      *                         3          ...

  public Dictionary<string, Ability> allAbilities;

      * All Abilities       key            value
      *                   "Drone"      SpecificAbility1
      *                   "Dash"       SpecificAbility2
      *                   "..."        SpecificAbility3

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
  private Button cancelButton;

  [SerializeField]
  private TextMeshProUGUI informationText;

  [SerializeField]
  private TextMeshProUGUI abilitiesCountText;

  [SerializeField]
  private TextMeshProUGUI abilityDescription;

  [SerializeField]
  private Button removeSlotOneButton;

  [SerializeField]
  private Button removeSlotTwoButton;

  [SerializeField]
  private Button removeSlotThreeButton;

  [Header("Ability Information")]
  [SerializeField]
  private GameObject abilityPrefab;

  [SerializeField]
  private GameObject abilitiesParent;

  public Ability magicSeed { get; private set; }
  public Ability summonPlague { get; private set; }
  public Ability specific2 { get; private set; }

  [Header("Ability Data")]
  [SerializeField]
  private D_BaseAbility magicSeedBaseData;

  [SerializeField]
  private D_MagicSeedAbility magicSeedData;

  [Space(5)]
  [SerializeField]
  private D_BaseAbility summonPlagueBaseData;

  [SerializeField]
  private D_SummonPlagueAbility summonPlagueData;

  [Space(5)]
  [SerializeField]
  private D_BaseAbility abilityTwoBaseData;

  [Header("Change Ability Information")]
  private bool isChangingPlayerAbility;
  private int slotToChange;

  [Header("Others")]
  [SerializeField]
  private Player player;
  private bool abilityListAlreadyHasData;

  void OnEnabled()
  {
      ChangeAbilityDescription(" ");
  }

  public void RemovePlayerAbility(int slot)
  {
      playerAbilities.Remove(slot);

      UpdateAbilityObject(GetPlayerSlotButton(slotToChange).gameObject, "Slot " + slot);
      ChangeRemovePlayerAbility(slot, false);
      GetPlayerSlotButton(slotToChange).GetComponent<ButtonDescription>().description = " ";

      ResetUI();
  }

  

  public void SelectUIAbility(string abilityName)
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

          Button _slotToUpdate = GetPlayerSlotButton(slotToChange);
          D_BaseAbility _newPlayerAbilityData = playerAbilities[slotToChange].baseAbilityData;

          UpdateAbilityObject(_slotToUpdate.gameObject, _newPlayerAbilityData.abilityName);

          _slotToUpdate.GetComponent<ButtonDescription>().description =
              _newPlayerAbilityData.description;

          ResetUI();
      }
      else { }
  }

  public void ResetUI()
  {
      ChangeInteractablePlayerAbilitySlots(true);

      closeButton.interactable = true;
      cancelButton.gameObject.SetActive(false);
      closeButton.gameObject.SetActive(true);

      GetPlayerSlotButton(slotToChange).Select();
      ChangeRemovePlayerAbility(slotToChange, false);

      informationText.text = "Abilities";

      isChangingPlayerAbility = false;
  }

  private Button GetPlayerSlotButton(int slotToChange)
  {
      switch (slotToChange)
      {
          case 1:
              return firstSlot;
          case 2:
              return secondSlot;
          case 3:
              return thirdSlot;
          default:
              return null;
      }
  }

  public void SelectUIPlayerAbility(int slot)
  {
      ChangeInteractablePlayerAbilitySlots(false);

      abilitiesParent.transform.GetChild(0).GetComponent<Button>().Select();

      isChangingPlayerAbility = true;
      slotToChange = slot;

      if (playerAbilities.ContainsKey(slot))
          ChangeRemovePlayerAbility(slot, true);
  }

  public void ListUIAbilities()
  {
      if (!abilityListAlreadyHasData)
      {
          foreach (Ability ability in allAbilities.Values)
          {
              GameObject _newAbilityObj = Instantiate(abilityPrefab);
              UpdateAbilityObject(_newAbilityObj, ability.baseAbilityData.abilityName);

              ButtonDescription descriptionComponent =
                  _newAbilityObj.GetComponent<ButtonDescription>();

              descriptionComponent.description = ability.baseAbilityData.description;
              descriptionComponent.abilityManager = this;

              _newAbilityObj.transform.SetParent(abilitiesParent.transform);
              _newAbilityObj.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

              _newAbilityObj
                  .GetComponent<Button>()
                  .onClick.AddListener(
                      delegate
                      {
                          SelectUIAbility(ability.baseAbilityData.abilityName);
                      }
                  );
          }

          abilityListAlreadyHasData = true;
      }
  }

  private void ChangeInteractablePlayerAbilitySlots(bool state)
  {
      firstSlot.interactable = state;
      secondSlot.interactable = state;
      thirdSlot.interactable = state;
  }

  private void ChangeRemovePlayerAbility(int slot, bool state)
  {
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
  }

  private void UpdateAbilityObject(GameObject abilityObj, string text)
  {
      abilityObj.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = text;
  }

  public void ChangeAbilityDescription(string description)
  {
      abilityDescription.text = description;
  }

  public int GetPlayerAbilitySlot(string abilityName)
  {
      foreach (KeyValuePair<int, Ability> playerAbility in playerAbilities)
      {
          if (playerAbility.Value == allAbilities[abilityName])
          {
              return playerAbility.Key;
          }
      }

      return 0;
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
  */
}
