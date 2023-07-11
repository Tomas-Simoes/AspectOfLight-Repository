using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Menu
{
    [Header("Default Menu")]
    [SerializeField]
    private GameObject defaultMenu;

    [SerializeField]
    private GameObject optionsMenu;
    private bool isInOptionsMenu = false;

    [SerializeField]
    private GameObject videoOptionsMenu;
    private bool isInVideoMenu = false;

    protected override void OnEnable()
    {
        base.OnEnable();

        EventManager.StartListening("onExitInPauseMenu", OnExitInPauseMenu);

        PauseGame(true);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onExitInPauseMenu", OnExitInPauseMenu);

        PauseGame(false);
    }

    public void PauseGame(bool pauseState)
    {
        Debug.Log("yaa");

        if (pauseState)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void OnExitInPauseMenu(Dictionary<string, object> message)
    {
        if (isInOptionsMenu)
        {
            ChangeOptionsMenuState(false);
            return;
        }

        if (isInVideoMenu)
        {
            ChangeVideoOptionsMenuState(false);
            return;
        }

        EventManager.TriggerEvent(
            "onPause",
            true,
            new Dictionary<string, object> { { "activeState", false } }
        );
    }

    public void OnContinuePressed()
    {
        EventManager.TriggerEvent(
            "onPause",
            true,
            new Dictionary<string, object> { { "activeState", false } }
        );
    }

    public void OnOptionsPressed()
    {
        ChangeOptionsMenuState(true);
    }

    public void OnVideoOptionsPressed()
    {
        ChangeVideoOptionsMenuState(true);
    }

    public void OnQuitToMenuPressed()
    {
        ScenesManager.Instance.LoadScene(sceneName: "MainMenu", isInGameTransition: false);
        GameManager.Instance.isPaused = false;
    }

    public void ChangeOptionsMenuState(bool activeState)
    {
        isInOptionsMenu = activeState;

        defaultMenu.SetActive(!activeState);
        optionsMenu.SetActive(activeState);
    }

    public void ChangeVideoOptionsMenuState(bool activeState)
    {
        isInVideoMenu = activeState;
        isInOptionsMenu = !activeState;

        optionsMenu.SetActive(!activeState);
        videoOptionsMenu.SetActive(activeState);
    }
}
