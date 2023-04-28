using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Singleton<PauseMenu>
{
    private void OnEnable()
    {
        EventManager.StartListening("onPause", OnPauseGame);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onPause", OnPauseGame);
    }

    public void OnPauseGame(Dictionary<string, object> message)
    {
        if ((bool)message["pauseGame"])
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void OnContinueClicked()
    {
        EventManager.TriggerEvent(
            "onPause",
            false,
            new Dictionary<string, object> { { "pauseGame", false } }
        );
    }
}
