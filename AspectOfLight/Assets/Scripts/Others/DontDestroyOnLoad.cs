using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static DontDestroyOnLoad instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogWarning(
                "Destroyed current object ("
                    + this.gameObject.name
                    + ") in scene "
                    + SceneManager.GetActiveScene().name
                    + " "
            );
            Destroy(this.gameObject);
            return;
        }
    }
}
