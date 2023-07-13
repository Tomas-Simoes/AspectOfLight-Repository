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

        EventManager.TriggerEvent(
            "onSceneLoadingOver",
            false,
            new Dictionary<string, object> { { "transitionType", transitionType } }
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

    /*
    private IEnumerator LoadSceneAsync(
        string sceneName,
        int outConnectionID,
        bool isInGameTransition,
        bool loadDataAfterLoadingScene,
        bool isRestartingGameInSave,
        bool loadSensitiveData
    )
    {
        Debug.LogWarning("Loading new scene with configurations: ");
        Debug.LogWarning("Scene Name: " + sceneName);
        Debug.LogWarning("Is in game transition: " + isInGameTransition);
        Debug.LogWarning("Load data after loading scene: " + loadDataAfterLoadingScene);
        Debug.LogWarning("Load sensitive data: " + loadSensitiveData);

        if (isInGameTransition)
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
                    break;
                }

                //Debug.LogWarning("Animation time: " + _animationTime);
                //Debug.LogWarning("Time" + Time.timeScale);
                yield return new WaitForSeconds(0.001f);
            }
        }

        if (isInGameTransition)
        {
            Debug.LogWarning("Set player position on out connectio: " + outConnectionID);
            SetPlayerPositionOnLoadedScene(sceneName, outConnectionID);
        }

        Debug.LogWarning("Starting async loading in scene " + sceneName + ".");

        var asyncLoadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!asyncLoadScene.isDone)
        {
            yield return null;
        }

        Debug.LogWarning("Async loading is done.");

        if (isInGameTransition)
            LoadSceneTransition();

        GameManager.Instance.gameStarted = sceneName == "MainMenu" ? false : true;

        if (sceneName != "MainMenu")
        {

            if (loadDataAfterLoadingScene)
                DataPersistenceManager.Instance.LoadGame(false, loadSensitiveData);
        }
        else
        {
            //TODO003
            /*
            IEnumerable<DontDestroyOnLoad> dontDestroyOnLoadObjects =
                FindObjectsOfType<MonoBehaviour>(true).OfType<DontDestroyOnLoad>();

            IEnumerable<DontDestroyPlayerOnLoad> dontDestroyPlayerOnLoadObject =
                FindObjectsOfType<MonoBehaviour>(true).OfType<DontDestroyPlayerOnLoad>();

            foreach (DontDestroyOnLoad item in dontDestroyOnLoadObjects)
            {
                Destroy(item.gameObject);
            }

            foreach (DontDestroyPlayerOnLoad item in dontDestroyPlayerOnLoadObject)
            {
                Destroy(item.gameObject);
            }
            
        }

        if (isInGameTransition)
        {
            CameraManager.Instance.SetUpDefaultCamera();
        }

        UIManager.Instance.ResetUIStates();
        GameManager.Instance.playerReference.inputHandler.ChangeMapStatesBasedOnUI(
            new Dictionary<string, object> { { "activeState", false } }
        );
        Debug.LogWarning("Scene loaded sucessfully.");
    }
    */

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

    private Animator LoadSceneTransition()
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
