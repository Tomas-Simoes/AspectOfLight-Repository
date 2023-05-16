using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : Singleton<DataPersistenceManager>
{
    [Header("Debugging")]
    [SerializeField]
    private bool initializeDataIfNull = false;

    [SerializeField]
    private bool disableDataPersitence = false;

    [SerializeField]
    private bool overrideSelectedProfileId = false;

    [SerializeField]
    private string testSelectedProfileId = "test";

    [Header("File Storage Config")]
    [SerializeField]
    private string fileName = "data.game";

    [SerializeField]
    private bool useEncryption;

    [SerializeField]
    public GameData gameData;
    private FileDataHandler dataHandler;

    [SerializeField]
    private string selectedProfileId = "";

    private List<IDataPersistence> dataPersistanceObjects = new List<IDataPersistence>();

    //! THIS IS FOR DEVELOPMENT ONLY
    //! REMOVE THIS WHEN BUILDING THE GAME
    public override void Init()
    {
        base.Init();

        LoadGame();
    }

    private void Awake()
    {
        if (disableDataPersitence)
        {
            Debug.LogWarning("Data Persitence is currently disabled.");
        }

        this.dataHandler = new FileDataHandler(
            Application.persistentDataPath,
            fileName,
            useEncryption
        );

        InitializeSelectedProfileId();
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.selectedProfileId = newProfileId;
        LoadGame();
    }

    public void DeleteProfileData(string profileId)
    {
        dataHandler.Delete(profileId);
        InitializeSelectedProfileId();
        LoadGame();
    }

    private void InitializeSelectedProfileId()
    {
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();

        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Override selected profile id with test id: " + testSelectedProfileId);
        }
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        SaveGame();
        GameManager.Instance.gameStarted = true;
    }

    public void SaveGame()
    {
        if (disableDataPersitence)
        {
            return;
        }

        if (this.gameData == null)
        {
            Debug.LogWarning(
                "No data was found. A new game needs to be started before data can be saved."
            );
            return;
        }

        dataPersistanceObjects = FindAllDataPersistenceObjects();

        foreach (IDataPersistence dataPersistenceObj in dataPersistanceObjects)
        {
            dataPersistenceObj.SaveData(gameData);

            foreach (KeyValuePair<string, bool> abilityLockState in gameData.abilityLockState)
            {
                Debug.Log(
                    "Key: "
                        + abilityLockState.Key
                        + "  /  "
                        + "Value: "
                        + abilityLockState.Value
                        + "  (DataPersistenceManager / on save)"
                );
            }
        }

        foreach (KeyValuePair<string, bool> abilityLockState in gameData.abilityLockState)
        {
            Debug.Log(
                "Key: "
                    + abilityLockState.Key
                    + "  /  "
                    + "Value: "
                    + abilityLockState.Value
                    + "  (DataPersistenceManager / after save)"
            );
        }
        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        dataHandler.Save(gameData, selectedProfileId);

        EventManager.TriggerEvent("onGameSaved", true, null);
    }

    public void LoadGame()
    {
        if (disableDataPersitence)
        {
            return;
        }

        this.gameData = dataHandler.Load(selectedProfileId);

        if (this.gameData == null)
        {
            Debug.LogWarning(
                "No data was found. A new game needs to be started before data can be loaded."
            );
            return;
        }

        dataPersistanceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistenceObj in dataPersistanceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistencesObjects = FindObjectsOfType<MonoBehaviour>(
                true
            )
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistencesObjects);
    }

    public bool HasGameData()
    {
        LoadGame();
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
