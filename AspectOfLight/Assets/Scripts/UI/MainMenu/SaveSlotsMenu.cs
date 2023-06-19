using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlotsMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField]
    private MainMenu mainMenu;

    [Header("Menu Buttons")]
    [SerializeField]
    private Button backButton;

    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        this.DeactiveMenu();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DisableMenuButtoms();

        DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        if (!isLoadingGame)
        {
            DataPersistenceManager.Instance.NewGame();

            ScenesManager.Instance.LoadScene(
                "ExiledFlorest",
                isInGameTransition: false,
                loadDataAfterLoadingScene: true
            );

            return;
        }

        ScenesManager.Instance.LoadScene(
            DataPersistenceManager.Instance.gameData.areaInMap,
            isInGameTransition: false,
            loadDataAfterLoadingScene: true
        );

        //DataPersistenceManager.Instance.SaveGame();
    }

    public void OnClearClicked(SaveSlot saveSlot)
    {
        DataPersistenceManager.Instance.DeleteProfileData(saveSlot.GetProfileId());
        ActivateMenu(isLoadingGame);
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        this.gameObject.SetActive(true);
        this.isLoadingGame = isLoadingGame;

        Dictionary<string, GameData> profilesGameData =
            DataPersistenceManager.Instance.GetAllProfilesGameData();

        GameObject firstSelected = backButton.gameObject;
        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);

            saveSlot.SetData(profileData);

            bool _isNotInteractable = profileData == null && isLoadingGame;

            saveSlot.SetInteractable(!_isNotInteractable);

            if (_isNotInteractable)
                saveSlot.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;

            if (!_isNotInteractable && firstSelected.Equals(backButton.gameObject))
            {
                firstSelected = saveSlot.gameObject;
            }
        }

        Button firstSelectedButton = firstSelected.GetComponent<Button>();
        firstSelectedButton.Select();
        //SetFirstSelected(firstSelectedButton);
    }

    public void DeactiveMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void DisableMenuButtoms()
    {
        foreach (SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }

        backButton.interactable = false;
    }
}
