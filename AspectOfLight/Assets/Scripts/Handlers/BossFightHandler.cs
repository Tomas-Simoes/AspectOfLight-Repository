using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class BossFightHandler : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField]
    private Animator cameraAnimator;

    [Header("Name")]
    [SerializeField]
    private TextMeshProUGUI bossName;

    [SerializeField]
    private Animator bossNameAnim;

    [SerializeField]
    private float fadeOutSeconds;

    [Header("Barriers")]
    [SerializeField]
    private GameObject backBarrier;

    [Header("Others")]
    private bool isFightStarted;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFightStarted)
        {
            cameraAnimator.SetBool("bossCamera", true);
            StartBossBattle();
        }
    }

    void StartBossBattle()
    {
        isFightStarted = true;

        InitializeText();

        backBarrier.SetActive(true);
    }

    IEnumerator FadeOutAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);

        bossNameAnim.SetBool("fadeIn", false);
        bossNameAnim.SetBool("fadeOut", true);
    }

    private void InitializeText()
    {
        bossNameAnim.SetBool("fadeOut", false);
        bossNameAnim.SetBool("fadeIn", true);

        StartCoroutine(FadeOutAfterSeconds(fadeOutSeconds));
    }
}
