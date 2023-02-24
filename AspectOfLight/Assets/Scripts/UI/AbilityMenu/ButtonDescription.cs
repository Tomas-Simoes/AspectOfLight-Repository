using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDescription : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Ability Manager")]
    public string description;

    [SerializeField]
    public AbilityManager abilityManager;

    public void OnSelect(BaseEventData eventData)
    {
        abilityManager.ChangeAbilityDescription(description);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        abilityManager.ChangeAbilityDescription(" ");
    }
}
