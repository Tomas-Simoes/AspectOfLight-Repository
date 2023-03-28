using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    }

    private void DisableButtonsDepedingOnData()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
            loadGameButton.interactable = false;
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
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadSceneAsync(DataPersistenceManager.instance.gameData.areaInMap);
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
