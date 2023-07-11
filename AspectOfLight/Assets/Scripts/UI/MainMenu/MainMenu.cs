using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField]
    private SaveSlotsMenu saveSlotsMenu;

    [Header("Menu Buttons")]
    [SerializeField]
    private Button newGameButton;

    [SerializeField]
    private Button continueGameButton;

    [SerializeField]
    private Button loadGameButton;

    void Start()
    {
        DisableButtonsDepedingOnData();

        GameManager.Instance.gameStarted = false;
    }

    private void DisableButtonsDepedingOnData()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
        {
            continueGameButton.interactable = false;
            loadGameButton.interactable = false;

            continueGameButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
            loadGameButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
        }
    }

    public void OnNewGameClicked()
    {
        saveSlotsMenu.ActivateMenu(false);
        this.DeactiveMenu();
    }

    public void OnLoadGameClicked()
    {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactiveMenu();
    }

    public void OnContinueClicked()
    {
        DisableMenuButtons();

        ScenesManager.Instance.LoadScene(
            DataPersistenceManager.Instance.gameData.areaInMap,
            loadDataAfterLoadingScene: true,
            loadSensitiveData: true
        );
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
        DisableButtonsDepedingOnData();
    }

    public void DeactiveMenu()
    {
        this.gameObject.SetActive(false);
    }
}
