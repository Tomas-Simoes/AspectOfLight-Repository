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

    void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
        }
    }

    public void OnNewGameClicked()
    {
        saveSlotsMenu.ActivateMenu();
        this.DeactiveMenu();
    }

    public void OnContinueClicked()
    {
        DisableMenuButtons();

        SceneManager.LoadSceneAsync("SampleScene");
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
    }

    public void DeactiveMenu()
    {
        this.gameObject.SetActive(false);
    }
}
