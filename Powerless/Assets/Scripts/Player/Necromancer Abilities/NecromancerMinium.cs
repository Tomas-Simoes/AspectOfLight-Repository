using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerMinium : MonoBehaviour
{

    [SerializeField]
    public GameObject SpawnerDrone;

    [SerializeField]
    public GameObject Drone;


    void Start()
    {
        Instantiate(Drone, transform);
    }
}