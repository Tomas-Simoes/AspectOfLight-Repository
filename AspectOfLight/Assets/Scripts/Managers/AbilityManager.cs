using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityManager : Singleton<AbilityManager>, IDataPersistence
{
    [Header("Abilities Information")]
    public D_Abilities abilitiesData;

    [Header("Player Abilities Information")]
    public static Dictionary<int, Ability> playerAbilities = new Dictionary<int, Ability>();

    [Header("Ability Information")]
    private float timeSinceAbilityOne;
    private float timeSinceAbilityTwo;
    private float timeSinceAbilityThree;

    [Header("Miracles")]
    public int playerMiracles = 3;

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
        abilitiesData = Resources.Load<D_Abilities>("UIElements/AbilitiesMenu/AbilitiesData");

        EventManager.StartListening("onAbilityCast", CastAbility);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onAbilityCast", CastAbility);
    }

    public void CastAbility(Dictionary<string, object> message)
    {
        int _currentSlot = (int)message["slot"];

        if (!playerAbilities.ContainsKey(_currentSlot))
            return;

        bool _hasStaminaToCast =
            (int)message["currentStamina"] - playerAbilities[_currentSlot].staminaCost >= 0;

        if (
            GetTimeSinceCast(_currentSlot) >= playerAbilities[_currentSlot].abilityCD
            && _hasStaminaToCast
        )
        {
            playerAbilities[_currentSlot].Cast((Player)message["player"]);

            switch (_currentSlot)
            {
                case 1:
                    timeSinceAbilityOne = 0;
                    break;
                case 2:
                    timeSinceAbilityTwo = 0;
                    break;
                case 3:
                    timeSinceAbilityThree = 0;
                    break;
            }
        }
        else
        {
            //TODO play some visual indicator that user has no stamina;
        }
    }

    public void RemovePlayerAbility(int slot)
    {
        playerAbilities.Remove(slot);

        //UpdateAbilityObject(GetPlayerSlotButton(slotToChange).gameObject, "Slot " + slot);
        //ChangeRemovePlayerAbility(slot, false);
        //GetPlayerSlotButton(slotToChange).GetComponent<ButtonDescription>().description = " ";
    }

    public bool UnlockAbility(Ability ability)
    {
        if (ability.unlockCost <= playerMiracles)
        {
            ability.isAvailable = true;
            playerMiracles -= ability.unlockCost;

            return true;
        }

        return false;
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

    public int GetPlayerAbilitySlot(string abilityName)
    {
        foreach (KeyValuePair<int, Ability> playerAbility in playerAbilities)
        {
            if (abilityName == playerAbility.Value.abilityName)
                return playerAbility.Key;
        }

        return 0;
    }

    public void LoadData(GameData data)
    {
        if (data.abilityLockState.Count != 0)
        {
            foreach (KeyValuePair<string, bool> abilityLockState in data.abilityLockState)
                foreach (Ability ability in abilitiesData.abilities)
                    if (ability.abilityName == abilityLockState.Key)
                        ability.isAvailable = abilityLockState.Value;
        }
    }

    public void SaveData(GameData data)
    {
        if (data.abilityLockState.Count == 0)
            foreach (Ability ability in abilitiesData.abilities)
                ability.isAvailable = false;

        foreach (Ability ability in abilitiesData.abilities)
        {
            if (data.abilityLockState.ContainsKey(ability.abilityName))
                data.abilityLockState[ability.abilityName] = ability.isAvailable;
            else
                data.abilityLockState.Add(ability.abilityName, ability.isAvailable);
        }
    }
}
