using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool gameStarted = false;

    private void OnEnable()
    {
        EventManager.StartListening("onNewGameStarted", OnNewGameStarted);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onNewGameStarted", OnNewGameStarted);
    }

    private void OnNewGameStarted(Dictionary<string, object> message)
    {
        AbilityManager.Instance.Init();
    }
}
