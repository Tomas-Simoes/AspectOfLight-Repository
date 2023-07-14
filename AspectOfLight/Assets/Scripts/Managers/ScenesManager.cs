using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class ScenesManager : Singleton<ScenesManager>
{
    private D_ScenesData scenesData;
    private GameObject sceneTransition;

    private string currentSceneTransitionType;

    public override void Init()
    {
        base.Init();
    }

    private void Start()
    {
        scenesData = Resources.Load<D_ScenesData>("Data/Scene Data/ScenesData");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadScene(
        string sceneName,
        string transitionType,
        int outConnectionID = 0,
        bool startingGame = false
    )
    {
        currentSceneTransitionType = transitionType;

        if (sceneTransition != null)
        {
            Destroy(sceneTransition);
        }

        StartCoroutine(LoadSceneAsync(sceneName, transitionType, outConnectionID, startingGame));
    }

    private IEnumerator LoadSceneAsync(
        string sceneName,
        string transitionType,
        int outConnectionID,
        bool startingGame
    )
    {
        Debug.LogWarning("Instantiate scene transition.");
        Animator _newSceneTransition = LoadSceneTransition();
        _newSceneTransition.SetBool("fadeIn", true);

        while (true)
        {
            float _animationTime = _newSceneTransition
                .GetCurrentAnimatorStateInfo(0)
                .normalizedTime;

            if (_animationTime >= 1)
            {
                goto FadeInTransitionOver;
            }

            //Debug.LogWarning("Animation time: " + _animationTime);
            //Debug.LogWarning("Time" + Time.timeScale);
            yield return new WaitForSeconds(0.001f);
        }
        FadeInTransitionOver:
        ;

        var asyncLoadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!asyncLoadScene.isDone)
        {
            yield return null;
        }
        Debug.LogWarning("Scene async loading is done.");
        Debug.LogWarning("Triggering \"onSceneLoadingOver\" event");

        D_ScenesData.GameScene _currentScene = scenesData.gameScenes.Find(
            scene => scene.name == sceneName
        );

        EventManager.TriggerEvent(
            "onSceneLoadingOver",
            false,
            new Dictionary<string, object>
            {
                { "transitionType", transitionType },
                { "sceneName", sceneName },
                { "playThemeOnEnter", _currentScene.audio.playThemeOnEnter },
                { "delayTime", _currentScene.audio.delayTime }
            }
        );

        if (transitionType == "InGameTransition")
            SetPlayerPositionOnLoadedScene(sceneName, outConnectionID);
        else
            AbilityManager.Instance.ForceAllAbilitiesCooldown();

        LoadSceneTransition();

        if (sceneName != "MainMenu")
        {
            EventManager.TriggerEvent("onPlayerUI", true, null);
        }

        if (transitionType != "UITransition" || startingGame)
        {
            CameraManager.Instance.SetUpDefaultCamera();
            GameManager.Instance.playerReference.inputHandler.ChangeMapStatesBasedOnUI(
                new Dictionary<string, object> { { "activeState", false } }
            );
        }

        if (startingGame)
            GameManager.Instance.gameStarted = true;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        UIManager.Instance.ResetUIStates();

        if (
            currentSceneTransitionType == "UITransition"
            || currentSceneTransitionType == "RestartTransition"
        )
        {
            DataPersistenceManager.Instance.LoadGame(loadSensitiveData: true);
        }

        if (currentSceneTransitionType == "InGameTransition")
        {
            DataPersistenceManager.Instance.LoadGame(loadSensitiveData: false);
        }
    }

    public void SetPlayerPositionOnLoadedScene(string sceneName, int outConnectionID)
    {
        D_ScenesData.GameScene newScene = null;

        foreach (D_ScenesData.GameScene gameScene in scenesData.gameScenes)
        {
            if (gameScene.name == sceneName)
            {
                newScene = gameScene;
            }
        }

        if (newScene == null)
        {
            Debug.LogWarning("There is no scene with the name " + sceneName);
            return;
        }

        foreach (D_ScenesData.SceneConnection sceneConnection in newScene.sceneConnections)
        {
            if (sceneConnection.connectionID == outConnectionID)
                GameManager.Instance.playerReference.transform.position = sceneConnection.position;
        }
    }

    public Animator LoadSceneTransition()
    {
        sceneTransition = Instantiate(
            Resources.Load<GameObject>("Data/Scene Data/SceneTransition")
        );

        Animator _newSceneTransition = sceneTransition.GetComponent<Animator>();

        _newSceneTransition.transform.SetParent(
            GameObject.FindGameObjectWithTag("Canvas").transform,
            false
        );

        return _newSceneTransition;
    }
}
