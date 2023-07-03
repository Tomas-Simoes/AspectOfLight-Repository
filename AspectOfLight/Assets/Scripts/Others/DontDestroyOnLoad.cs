using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static GameObject instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Destroyed instance: " + instance);
            Destroy(instance);
        }

        instance = gameObject;
        instance.name = "New Ability Instance";
        DontDestroyOnLoad(this.gameObject);
    }
}
