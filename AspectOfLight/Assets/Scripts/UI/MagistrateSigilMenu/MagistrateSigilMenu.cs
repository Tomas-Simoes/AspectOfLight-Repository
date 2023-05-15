using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagistrateSigilMenu : Menu
{
    bool firstTimeRest = true;

    public void OnRestPressed()
    {
        EventManager.TriggerEvent(
            "onRest",
            false,
            new Dictionary<string, object> { { "firstTimeRest", firstTimeRest } }
        );

        DataPersistenceManager.Instance.SaveGame();
        firstTimeRest = false;
    }

    public void OnSacredPathsPressed()
    {
        EventManager.TriggerEvent(
            "onAbilityTree",
            true,
            new Dictionary<string, object> { { "activeState", true } }
        );
    }

    public void OnSaveGamePressed()
    {
        DataPersistenceManager.Instance.SaveGame();
    }

    public void OnLoadGamePressed() { }
}
