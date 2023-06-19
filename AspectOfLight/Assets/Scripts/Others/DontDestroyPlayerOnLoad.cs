using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyPlayerOnLoad : MonoBehaviour
{
    private static GameObject instance;

    void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = gameObject;
        GameManager.Instance.playerReference = instance.GetComponentInChildren<Player>();

        DontDestroyOnLoad(this);
    }
}
