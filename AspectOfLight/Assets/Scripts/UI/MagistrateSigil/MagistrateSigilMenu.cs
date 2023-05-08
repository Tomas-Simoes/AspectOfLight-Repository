using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagistrateSigilMenu : Singleton<MagistrateSigilMenu>
{
    public void OnRestPressed() { }

    public void OnSkillTreePressed()
    {
        UIManager.Instance.ChangeUIAbilityTreeState(true);
    }

    public void OnSaveGamePressed()
    {
        DataPersistenceManager.instance.SaveGame();
    }

    public void OnLoadGamePressed() { }
}
