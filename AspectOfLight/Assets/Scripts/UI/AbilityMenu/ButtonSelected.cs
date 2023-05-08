using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelected : MonoBehaviour, ISelectHandler, IDeselectHandler
{
  [Header("Ability")]
  public Ability ability;

  public void OnSelect(BaseEventData eventData)
  {
    if (ability != null)
    {
      EventManager.TriggerEvent(
          "onSelectAbility",
          true,
          new Dictionary<string, object>
          {
                { "icon", ability.iconAvailable },
                { "name", ability.abilityName },
                { "text", ability.description }
          }
      );
    }
  }

  public void OnDeselect(BaseEventData eventData)
  {
    EventManager.TriggerEvent("onDeselectAbility", true, null);
  }
}
