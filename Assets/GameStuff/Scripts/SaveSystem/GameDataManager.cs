using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameDataManager : MonoBehaviour
{
    public string dataFileName;
    public bool encryptData = false;

    public static GameDataManager instance;

    private List<ISaveable> saveableObjects = new List<ISaveable>();

    private DataFileHandler fileHandler;

    private GameData gameData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of GameDataManager");
        }
    }

    private void Start()
    {
        fileHandler = new DataFileHandler(Application.persistentDataPath, dataFileName, encryptData);
        saveableObjects = FindAllSaveableObjects();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void SaveGame()
    {
        foreach (var obj in saveableObjects)
        {
            obj.Save(ref gameData);
        }

        fileHandler.Save(gameData);
    }

    public void LoadGame()
    {
        gameData = fileHandler.Load();

        if (gameData == null)
        {
            NewGame();
        }
        else
        {
            foreach (var obj in saveableObjects)
            {
                obj.Load(gameData);
            }
        }
    }

    private List<ISaveable> FindAllSaveableObjects()
    {
        var objects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();
        return objects.ToList();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
