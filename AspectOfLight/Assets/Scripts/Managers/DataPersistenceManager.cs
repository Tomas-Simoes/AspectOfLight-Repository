using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : Singleton<DataPersistenceManager>
{
    [Header("Debugging")]
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

    public override void Init()
    {
        base.Init();

        if (GameManager.Instance.environment == "development")
        {
            LoadGame(false);
            GameManager.Instance.gameStarted = true;

            if (UIManager.Instance.playerUI == null)
            {
                EventManager.TriggerEvent("onPlayerUI", true, null);
            }
        }
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

        GameManager.Instance.Init();
        EventManager.TriggerEvent("onNewGameStarted", false, null);

        Debug.LogWarning("A new game was created.");
        SaveGame(true);
    }

    public void SaveGame(bool isNewGame = false)
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

        if (!isNewGame)
        {
            dataPersistanceObjects = FindAllDataPersistenceObjects();

            foreach (IDataPersistence dataPersistenceObj in dataPersistanceObjects)
            {
                dataPersistenceObj.SaveData(gameData);
            }
        }

        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        dataHandler.Save(gameData, selectedProfileId);

        EventManager.TriggerEvent("onGameSaved", true, null);
        Debug.LogWarning("The game was saved sucessfully.");
    }

    public void LoadGame(bool isAfterChangingScene = true, bool loadSensitiveData = false)
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
            dataPersistenceObj.LoadData(gameData, loadSensitiveData);
        }

        Debug.LogWarning("The game was loaded sucessfully.");
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
