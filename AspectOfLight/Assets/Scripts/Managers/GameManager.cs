using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool gameStarted = false;
    public Player playerReference;

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
        CameraManager.Instance.Init();
    }

    public void SavePlayerReference(Player player)
    {
        playerReference = player;
    }
}
