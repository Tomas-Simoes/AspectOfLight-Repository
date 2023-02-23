using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDescription : MonoBehaviour, ISelectHandler
{
    [Header("Ability Manager")]
    public string description;

    [SerializeField]
    private AbilityManager abilityManager;

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(description + " ------ ");
        abilityManager.ChangeDescription(description);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //abilityManager.ChangeDescription(" ");
    }
}
