using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Menu
{
    protected override void OnEnable()
    {
        base.OnEnable();

        PauseGame(true);
    }

    private void OnDisable()
    {
        PauseGame(false);
    }

    public void PauseGame(bool pauseState)
    {
        if (pauseState)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void OnContinuePressed()
    {
        EventManager.TriggerEvent(
            "onPause",
            true,
            new Dictionary<string, object> { { "activeState", false } }
        );
    }

    public void OnOptionsPressed() { }

    public void OnQuitToMenuPressed()
    {
        ScenesManager.Instance.LoadScene(sceneName: "MainMenu", isInGameTransition: false);
    }
}
