using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
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
    private string fileName;

    [SerializeField]
    private bool useEncryption;

    public GameData gameData;
    private FileDataHandler dataHandler;

    private string selectedProfileId = "";

    private List<IDataPersistence> dataPersistanceObjects;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log(
                "Found more than one Data Persistence Manager in the scene. Destroying the newest one."
            );
            Destroy(this.gameObject);
            return;
        }

        instance = this;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistanceObjects = FindAllDataPersistenceObjects();
        LoadGame();
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

        foreach (IDataPersistence dataPersistenceObj in dataPersistanceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }
        gameData.lastUpdated = System.DateTime.Now.ToBinary();
        dataHandler.Save(gameData, selectedProfileId);

        Debug.Log("Saved profile: " + selectedProfileId);
    }

    public void LoadGame()
    {
        if (disableDataPersitence)
        {
            return;
        }

        this.gameData = dataHandler.Load(selectedProfileId);

        //? This is only for debugging reasons
        if (this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        if (this.gameData == null)
        {
            Debug.LogWarning(
                "No data was found. A new game needs to be started before data can be loaded."
            );
            return;
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistanceObjects)
        {
            Debug.Log("Loading data ");
            dataPersistenceObj.LoadData(gameData);
        }
    }

    /*
    ? Only for testing purpouses
    private void OnApplicationQuit()
    {
        //SaveGame();
    }
    */
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
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
