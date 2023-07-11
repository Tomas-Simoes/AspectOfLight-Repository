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

    public override void Init()
    {
        base.Init();
    }

    private void Start()
    {
        scenesData = Resources.Load<D_ScenesData>("Data/Scene Data/ScenesData");
    }

    public void LoadScene(
        string sceneName,
        int outConnectionID = 0,
        bool isInGameTransition = true,
        bool loadDataAfterLoadingScene = true
    )
    {
        if (sceneTransition != null)
        {
            Destroy(sceneTransition);
        }

        StartCoroutine(
            LoadSceneAsync(
                sceneName,
                outConnectionID,
                isInGameTransition,
                loadDataAfterLoadingScene
            )
        );
    }

    private IEnumerator LoadSceneAsync(
        string sceneName,
        int outConnectionID,
        bool isInGameTransition,
        bool loadDataAfterLoadingScene
    )
    {
        if (isInGameTransition)
        {
            Animator _newSceneTransition = LoadSceneTransition();
            _newSceneTransition.SetBool("fadeIn", true);

            while (true)
            {
                float _animationTime = _newSceneTransition
                    .GetCurrentAnimatorStateInfo(0)
                    .normalizedTime;

                if (_animationTime >= 1)
                    break;

                yield return new WaitForSeconds(0.001f);
            }
        }

        var asyncLoadScene = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoadScene.isDone)
        {
            yield return null;
        }

        if (isInGameTransition)
            LoadSceneTransition();

        GameManager.Instance.gameStarted = sceneName == "MainMenu" ? false : true;

        if (sceneName != "MainMenu")
        {
            EventManager.TriggerEvent("onPlayerUI", true, null);

            if (loadDataAfterLoadingScene)
                DataPersistenceManager.Instance.LoadGame(isInGameTransition);
        }
        else
        {
            //TODO003

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
            SetPlayerPositionOnLoadedScene(sceneName, outConnectionID);
            CameraManager.Instance.SetUpDefaultCamera();
        }

        UIManager.Instance.ResetUIStates();
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
