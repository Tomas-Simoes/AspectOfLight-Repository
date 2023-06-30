using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : Menu
{
    [Header("Death UI information")]
    [SerializeField]
    private TextMeshProUGUI deathText;

    [SerializeField]
    private string[] deathMessages;

    protected override void OnEnable()
    {
        base.OnEnable();

        deathText.text = deathMessages[UnityEngine.Random.Range(0, deathMessages.Length)];
        Time.timeScale = 0;
    }

    public void OnContinuePressed()
    {
        Time.timeScale = 1f;

        ScenesManager.Instance.LoadScene(
            DataPersistenceManager.Instance.gameData.areaInMap,
            loadDataAfterLoadingScene: true
        );
    }

    public void OnExitToMenuPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
