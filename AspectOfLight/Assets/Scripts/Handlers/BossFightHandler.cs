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
    private Enemy boss;

    [Header("Camera")]
    [SerializeField]
    private Animator cameraAnimator;

    [Header("Name")]
    [SerializeField]
    private TextMeshProUGUI bossName;

    [SerializeField]
    private Animator bossNameAnim;

    [SerializeField]
    private float awakeSeconds;

    [Header("Barrier")]
    [SerializeField]
    private GameObject backBarrier;

    [Header("UI Elements")]
    [SerializeField]
    private GameObject bossArenaUI;

    [SerializeField]
    private GameObject bossInformationUI;

    [SerializeField]
    private Slider healthSlider;

    private void Update()
    {
        UpdateBossHealthBar();
    }

    private void StartBossBattle()
    {
        bossArenaUI.SetActive(true);

        InitializeText();

        backBarrier.SetActive(true);
    }

    private void InitializeText()
    {
        bossNameAnim.SetBool("fadeOut", false);
        bossNameAnim.SetBool("fadeIn", true);

        StartCoroutine(AwakeBoss(awakeSeconds));
    }

    private void UpdateBossHealthBar()
    {
        healthSlider.value = (float)boss.currentHealth / (float)boss.entityData.MAX_HEALTH;
    }

    IEnumerator AwakeBoss(float time)
    {
        yield return new WaitForSeconds(time);

        bossNameAnim.SetBool("fadeIn", false);
        bossNameAnim.SetBool("fadeOut", true);

        boss.isFightStarted = true;
        bossInformationUI.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !boss.isFightStarted)
        {
            cameraAnimator.SetBool("bossCamera", true);
            StartBossBattle();
        }
    }
}
