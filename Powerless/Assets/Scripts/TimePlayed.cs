using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePlayed : MonoBehaviour
{
    [Header("Time Information")]
    [SerializeField]
    private float timePlayed;

    void Update()
    {
        timePlayed += Time.deltaTime;
    }
}
