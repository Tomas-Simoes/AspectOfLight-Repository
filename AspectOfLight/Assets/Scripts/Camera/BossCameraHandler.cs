using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossCameraHandler : MonoBehaviour
{
    [SerializeField]
    private Animator cameraAnimator;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cameraAnimator.SetBool("bossCamera", true);
        }
    }
}
