using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelected : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Ability ability;
    public bool isSelected;

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;

        if (ability != null)
        {
            EventManager.TriggerEvent(
                "onSelectAbility",
                true,
                new Dictionary<string, object>
                {
                    { "icon", ability.iconAvailable },
                    { "name", ability.abilityName },
                    {
                        "text",
                        !ability.isAvailable
                            ? "This ability needs "
                                + ability.unlockCost
                                + " miracles to unlock. \n\n"
                                + ability.description
                            : ability.description
                    }
                }
            );
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;

        EventManager.TriggerEvent("onDeselectAbility", true, null);
    }
}
