using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreatedAtRuntimeObject : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += DestroyItselfOnLoadedScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= DestroyItselfOnLoadedScene;
    }

    private void DestroyItselfOnLoadedScene(Scene scene, LoadSceneMode sceneMode)
    {
        Destroy(this.gameObject);
    }
}
