using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Death UI")]
    [SerializeField]
    private GameObject deathScreen;

    [SerializeField]
    private TextMeshProUGUI deathText;

    [SerializeField]
    private string[] deathMessages;

    public void OpenDeathScreen()
    {
        if (deathScreen == null)
        {
            D_DeathScreen deathScreenData = Resources.Load<D_DeathScreen>(
                "Death Screen/DeathScreenData"
            );

            deathScreen = Instantiate(deathScreenData.deathScreen);
            deathMessages = deathScreenData.deathMessages;

            deathScreen.transform.SetParent(
                GameObject.FindGameObjectWithTag("Canvas").transform,
                false
            );

            deathScreen.transform
                .Find("ContinueButton")
                .GetComponent<Button>()
                .onClick.AddListener(OnDeathUIContinueClicked);

            deathText = deathScreen.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            deathScreen.SetActive(false);
        }

        Time.timeScale = 0f;

        deathScreen.SetActive(true);
        deathText.text = deathMessages[UnityEngine.Random.Range(0, deathMessages.Length)];
    }

    public void OnDeathUIContinueClicked()
    {
        SceneManager.LoadScene(DataPersistenceManager.instance.gameData.areaInMap);
        Time.timeScale = 1f;
    }

    public void OnMagistrateSigilRestPressed() { }

    public void OnMagistrateSigilSkillTreePressed() { }

    public void OnMagistrateSigilSaveGamePressed()
    {
        DataPersistenceManager.instance.SaveGame();
    }

    public void OnMagistrateSigilLoadGamePressed() { }
}
