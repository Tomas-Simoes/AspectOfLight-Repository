using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagistrateSigilMenu : Singleton<MagistrateSigilMenu>
{
    public void OnRestPressed() { }

    public void OnSkillTreePressed()
    {
        EventManager.TriggerEvent(
            "onAbilityTree",
            true,
            new Dictionary<string, object> { { "activeState", true } }
        );
    }

    public void OnSaveGamePressed()
    {
        DataPersistenceManager.instance.SaveGame();
    }

    public void OnLoadGamePressed() { }
}
