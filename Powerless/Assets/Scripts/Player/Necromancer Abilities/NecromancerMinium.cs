using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerMinium : MonoBehaviour
{

    [SerializeField]
    public GameObject SpawnerDrone;

    [SerializeField]
    public GameObject Drone;
    public void SpawnDrone()
    {
        Instantiate(Drone, transform);
    }
}