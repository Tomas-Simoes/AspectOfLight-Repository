using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyPlayerOnLoad : MonoBehaviour
{
    private static DontDestroyPlayerOnLoad instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            GameManager.Instance.playerReference =
                instance.gameObject.GetComponentInChildren<Player>();

            DontDestroyOnLoad(this.gameObject);
            this.gameObject.name = this.gameObject.name + " (DDOL)";
        }
        else
        {
            Debug.LogWarning(
                "Destroyed current player ("
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
