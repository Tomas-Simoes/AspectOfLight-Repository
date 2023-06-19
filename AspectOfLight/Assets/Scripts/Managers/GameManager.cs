using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("In Game Information")]
    public bool gameStarted = false;
    public bool onGodMode = false;

    [Header("Class Information")]
    public Player playerReference;

    private void OnEnable()
    {
        EventManager.StartListening("onNewGameStarted", OnNewGameStarted);
        EventManager.StartListening("onGodMode", OnGodMode);
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

    public void CreateForceCameraRatiosObject()
    {
        GameObject forceCameraRatio = Resources.Load<GameObject>("Camera/Force Camera Ratios");
        GameObject forceCameraRatioObj = Instantiate(forceCameraRatio);

        forceCameraRatioObj.name = "--- Force Camera Ratio";
    }

    public void OnGodMode(Dictionary<string, object> message)
    {
        onGodMode = !onGodMode;
    }
}
