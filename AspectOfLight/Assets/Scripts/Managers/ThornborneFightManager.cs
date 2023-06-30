using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class ThornborneFightManager : MonoBehaviour, IDataPersistence
{
    [Header("Boss Battle Information")]
    [SerializeField]
    private Enemy boss;

    [SerializeField]
    private string bossName;

    [SerializeField]
    private bool bossAlreadyDefeated;

    [Header("Boss Battle Texts")]
    [SerializeField]
    private Animator bossNameText;

    [SerializeField]
    private Animator bossDefeatedText;

    [SerializeField]
    private float awakeSeconds;

    [Header("Camera")]
    [SerializeField]
    private Animator cameraAnimator;

    [Header("Barriers")]
    [SerializeField]
    private Spikes backSpikes1;

    [SerializeField]
    private Spikes backSpikes2;

    [SerializeField]
    private Spikes engineerVillageSpikes;

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

    private void OnEnable()
    {
        EventManager.StartListening("onThornborneDie", OnBossDefeated);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onThornborneDie", OnBossDefeated);
    }

    private void Start()
    {
        if (bossAlreadyDefeated)
        {
            Destroy(boss.gameObject);

            Destroy(backSpikes1.gameObject);
            Destroy(backSpikes2.gameObject);

            Destroy(engineerVillageSpikes.gameObject);

            this.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void StartBossBattle()
    {
        ChangeBarriersState(true);
        bossArenaUI.SetActive(true);

        boss.isFightStarted = true;

        CameraManager.Instance.ChangeCamera(cameraAnimator, "bossCamera", true);

        FadeInText(bossNameText);
        StartCoroutine(AwakeBoss(awakeSeconds));
    }

    IEnumerator AwakeBoss(float time)
    {
        yield return new WaitForSeconds(time);

        FadeOutText(bossNameText);
        bossInformationUI.SetActive(true);
    }

    private void UpdateBossHealthBar()
    {
        healthSlider.value = (float)boss.currentHealth / (float)boss.entityData.MAX_HEALTH;
    }

    private void OnBossDefeated(Dictionary<string, object> message)
    {
        bossInformationUI.SetActive(false);

        FadeInText(bossDefeatedText);
        StartCoroutine(EndBossBattle());
    }

    IEnumerator EndBossBattle()
    {
        yield return new WaitForSeconds(2);

        FadeOutText(bossDefeatedText);

        bossInformationUI.SetActive(false);
        ChangeBarriersState(false);

        yield return new WaitForSeconds(0.5f);

        CameraManager.Instance.ChangeCamera(cameraAnimator, "barrierCamera", true);

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(engineerVillageSpikes.Go("Down"));

        yield return new WaitForSeconds(0.7f);

        CameraManager.Instance.ChangeCamera(cameraAnimator, "mainCamera", true);

        cameraAnimator.SetBool("barrierCamera", false);
        cameraAnimator.SetBool("bossCamera", false);

        bossAlreadyDefeated = true;
        DataPersistenceManager.Instance.SaveGame();
    }

    private void ChangeBarriersState(bool isFightStarting)
    {
        if (isFightStarting)
        {
            backSpikes1.gameObject.SetActive(true);
            backSpikes2.gameObject.SetActive(true);

            Debug.Log("Starting up coroutine");
            StartCoroutine(backSpikes1.Go("Up"));
            StartCoroutine(backSpikes2.Go("Up"));
            return;
        }
        
        Debug.Log("Starting down coroutine");
        StartCoroutine(backSpikes1.Go("Down"));
        StartCoroutine(backSpikes2.Go("Down"));
    }

    private void FadeInText(Animator text)
    {
        text.gameObject.SetActive(true);

        text.SetBool("fadeOut", false);
        text.SetBool("fadeIn", true);
    }

    private void FadeOutText(Animator text)
    {
        text.SetBool("fadeIn", false);
        text.SetBool("fadeOut", true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !boss.isFightStarted)
        {
            StartBossBattle();
        }
    }

    public void LoadData(GameData data)
    {
        bossAlreadyDefeated = data.bosses[bossName];

        if (bossAlreadyDefeated)
        {
            Destroy(boss.gameObject);
            Destroy(backSpikes1.gameObject);
            Destroy(engineerVillageSpikes.gameObject);

            this.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void SaveData(GameData data)
    {
        data.bosses[bossName] = bossAlreadyDefeated;
    }
}
