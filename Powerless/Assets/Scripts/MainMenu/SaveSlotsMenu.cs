using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSlotsMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField]
    private MainMenu mainMenu;

    private SaveSlot[] saveSlots;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        this.DeactiveMenu();
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);

        Dictionary<string, GameData> profilesGameData =
            DataPersistenceManager.instance.GetAllProfilesGameData();
        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);

            saveSlot.SetData(profileData);
        }
    }

    public void DeactiveMenu()
    {
        this.gameObject.SetActive(false);
    }
}
