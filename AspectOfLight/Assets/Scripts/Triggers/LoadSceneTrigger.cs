using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneTrigger : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    [SerializeField]
    private int outConnectionID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScenesManager.Instance.LoadScene(sceneName, "InGameTransition", outConnectionID);
        }
    }
}
