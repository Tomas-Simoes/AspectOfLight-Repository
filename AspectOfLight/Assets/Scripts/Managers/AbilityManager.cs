using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class AbilityManager : Singleton<AbilityManager>, IDataPersistence
{
    [Header("Abilities Information")]
    public D_Abilities abilitiesData;

    [Header("Player Abilities Information")]
    public static Dictionary<int, Ability> playerAbilities = new Dictionary<int, Ability>();

    [Header("Player Ability Information")]
    private Ability currentCastAbility;
    private float timeSinceAbilityOne;
    private float timeSinceAbilityTwo;
    private float timeSinceAbilityThree;
    private float timeSinceAbilityFour;
    private float timeSinceAbilityGotCasted;
    private float minTimeSinceAbilityGotCasted = 0.75f;

    [Header("Miracles")]
    public int currentMiracles = 4;

    private void Update()
    {
        UpdateCooldowns();

        if (currentCastAbility != null)
        {
            currentCastAbility.LogicUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (currentCastAbility != null)
        {
            currentCastAbility.PhysicsUpdate();
        }
    }

    private void UpdateCooldowns()
    {
        timeSinceAbilityOne += Time.deltaTime;
        timeSinceAbilityTwo += Time.deltaTime;
        timeSinceAbilityThree += Time.deltaTime;
        timeSinceAbilityFour += Time.deltaTime;

        timeSinceAbilityGotCasted += Time.deltaTime;
    }

    private void OnEnable()
    {
        abilitiesData = Resources.Load<D_Abilities>("UIElements/AbilitiesMenu/AbilitiesData");

        EventManager.StartListening("onAbilityCast", CastAbility);
        EventManager.StartListening("onStopAbilityCast", StopCastAbility);
        EventManager.StartListening("onAbilityResetCooldown", ResetAbilityCooldown);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onAbilityCast", CastAbility);
        EventManager.StopListening("onStopAbilityCast", StopCastAbility);
        EventManager.StopListening("onAbilityResetCooldown", ResetAbilityCooldown);
    }

    public void CastAbility(Dictionary<string, object> message)
    {
        int _currentSlot = (int)message["slot"];
        if (!playerAbilities.ContainsKey(_currentSlot))
            return;

        bool _hasStaminaToCast =
            (int)message["currentStamina"] - playerAbilities[_currentSlot].staminaCost >= 0;

        bool _canCastAbility =
            (
                GetTimeSinceCast(_currentSlot) >= playerAbilities[_currentSlot].abilityCD
                && timeSinceAbilityGotCasted >= minTimeSinceAbilityGotCasted
            ) && _hasStaminaToCast;

        if (!_canCastAbility)
        {
            return;
        }

        timeSinceAbilityGotCasted = 0;

        currentCastAbility = playerAbilities[_currentSlot];
        Ability abilityGettingCastedInDedicatedState = message["abilityGettingCasted"] as Ability;

        if (abilityGettingCastedInDedicatedState == null)
        {
            currentCastAbility.Cast((Player)message["player"]);
            return;
        }

        foreach (KeyValuePair<int, Ability> playerAbility in playerAbilities)
        {
            if (
                playerAbility.Value == abilityGettingCastedInDedicatedState
                && playerAbility.Value.canBeRecasted
            )
            {
                currentCastAbility.Cast((Player)message["player"]);
                return;
            }
        }
    }

    public void StopCastAbility(Dictionary<string, object> message)
    {
        currentCastAbility = null;
    }

    public void ResetAbilityCooldown(Dictionary<string, object> message)
    {
        foreach (KeyValuePair<int, Ability> playerAbility in playerAbilities)
        {
            if (playerAbility.Value.abilityName == (string)message["abilityName"])
            {
                switch (playerAbility.Key)
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
                    case 4:
                        timeSinceAbilityFour = 0;
                        break;
                }
            }
        }
    }

    public void ForcebilityCooldown(string name)
    {
        foreach (KeyValuePair<int, Ability> playerAbility in playerAbilities)
        {
            if (playerAbility.Value.abilityName == name)
            {
                switch (playerAbility.Key)
                {
                    case 1:
                        timeSinceAbilityOne = playerAbility.Value.abilityCD;
                        break;
                    case 2:
                        timeSinceAbilityTwo = playerAbility.Value.abilityCD;
                        break;
                    case 3:
                        timeSinceAbilityThree = playerAbility.Value.abilityCD;
                        break;
                    case 4:
                        timeSinceAbilityFour = playerAbility.Value.abilityCD;
                        break;
                }
            }
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
        if (CanUnlockAbility(ability))
        {
            ability.isAvailable = true;
            currentMiracles -= ability.unlockCost;

            return true;
        }

        return false;
    }

    public bool CanUnlockAbility(Ability ability)
    {
        return ability.unlockCost <= currentMiracles || GameManager.Instance.onGodMode;
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
            case 4:
                return timeSinceAbilityFour;
        }

        Debug.LogWarning("There is no variable with the cooldown for that slot.");
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

    public void LoadData(GameData data, bool loadSensitiveData)
    {
        if (data.abilityLockState.Count != 0)
        {
            foreach (KeyValuePair<string, bool> abilityLockState in data.abilityLockState)
                foreach (Ability ability in abilitiesData.abilities)
                    if (ability.abilityName == abilityLockState.Key)
                        ability.isAvailable = abilityLockState.Value;
        }

        playerAbilities = new Dictionary<int, Ability>();

        foreach (KeyValuePair<int, string> savedPlayerAbility in data.playerAbilities)
        {
            foreach (Ability ability in abilitiesData.abilities)
                if (ability.abilityName == savedPlayerAbility.Value)
                    playerAbilities.Add(savedPlayerAbility.Key, ability);
        }

        currentMiracles = data.currentMiracles;
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

        data.playerAbilities = new Dictionary<int, string>();

        foreach (KeyValuePair<int, Ability> playerAbility in playerAbilities)
        {
            data.playerAbilities.Add(playerAbility.Key, playerAbility.Value.abilityName);
        }

        data.currentMiracles = currentMiracles;
    }
}
