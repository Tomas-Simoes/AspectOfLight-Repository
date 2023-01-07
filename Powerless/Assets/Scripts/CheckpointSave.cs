using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSave : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DataPersistenceManager.instance.SaveGame();
        }
    }
}
