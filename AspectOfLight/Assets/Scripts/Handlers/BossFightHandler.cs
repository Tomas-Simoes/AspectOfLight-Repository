using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class BossFightHandler : MonoBehaviour
{
    [Header("Boss Battle Information")]
    [SerializeField]
    private Entity boss;
    private bool isFightStarted;

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

    [Header("Barrier")]
    [SerializeField]
    private GameObject backBarrier;

    [Header("UI Elements")]
    [SerializeField]
    private GameObject bossArenaUI;

    [SerializeField]
    private Slider healthSlider;

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

        bossArenaUI.SetActive(true);
        InitializeText();

        backBarrier.SetActive(true);
    }

    private void Update()
    {
        UpdateBossHealthBar();
    }

    private void InitializeText()
    {
        bossNameAnim.SetBool("fadeOut", false);
        bossNameAnim.SetBool("fadeIn", true);

        StartCoroutine(FadeOutAfterSeconds(fadeOutSeconds));
    }

    private void UpdateBossHealthBar()
    {
        healthSlider.value = (float)boss.currentHealth / (float)boss.entityData.MAX_HEALTH;
    }

    IEnumerator FadeOutAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);

        bossNameAnim.SetBool("fadeIn", false);
        bossNameAnim.SetBool("fadeOut", true);
    }
}
