using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField]
    private string fileName;

    [SerializeField]
    private bool useEncryption;

    private GameData gameData;
    private List<IDataPersistence> dataPersistanceObjects;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene");
        }

        instance = this;
    }

    //? Only for testing purpouses
    private void Start()
    {
        this.dataHandler = new FileDataHandler(
            Application.persistentDataPath,
            fileName,
            useEncryption
        );
        this.dataPersistanceObjects = FindAllDataPersistenceObjects();

        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistanceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
        Debug.Log("Saved Successfully");
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistanceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        Debug.Log("Loaded Successfully");
    }

    //? Only for testing purpouses
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistencesObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistencesObjects);
    }
}
