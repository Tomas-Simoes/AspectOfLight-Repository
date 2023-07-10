using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Off Game Information")]
    public string environment = "production";

    [Header("In Game Information")]
    public int deathCount = 0;
    public float timePlayed = 0;
    public bool gameStarted = false;
    public bool isPaused = false;
    public bool onGodMode = false;

    [Header("Settings")]
    public Resolution currentResolution;
    public int currentTargetFrameRate = 60;

    [Header("Class Information")]
    public Player playerReference;

    private void OnEnable()
    {
        Application.targetFrameRate = currentTargetFrameRate;

        EventManager.StartListening("onNewGameStarted", OnNewGameStarted);
        EventManager.StartListening("onGodMode", OnGodMode);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onNewGameStarted", OnNewGameStarted);
        EventManager.StopListening("onGodMode", OnGodMode);
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
