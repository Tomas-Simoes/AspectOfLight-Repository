using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MagistrateSigilMenu : Menu
{
    bool firstTimeRest = true;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

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

    //TODO001
    public void OnDemonstrationScenePressed()
    {
        if (SceneManager.GetActiveScene().name == "Demonstration")
        {
            ScenesManager.Instance.LoadScene("ExiledFlorest", "InGameTransition", 2);
            DeactivateMenu();
            return;
        }

        ScenesManager.Instance.LoadScene("Demonstration", "InGameTransition", 1);
        DeactivateMenu();
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }
}
