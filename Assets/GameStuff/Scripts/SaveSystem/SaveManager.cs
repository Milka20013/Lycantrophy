using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string folderName = "GameData";

    private string saveFolder;

    private delegate void SaveHandler();
    private SaveHandler OnSaveGame;
    private SaveHandler OnLoadGame;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than one instances of SaveManager");
            //What now?
        }

        saveFolder = Path.Combine(Application.persistentDataPath, folderName);
    }

    public void Delete(string profileName)
    {
        string path = Path.Combine(saveFolder, profileName);
        if (!File.Exists(path))
        {
            throw new Exception($"Save profile {profileName} doesn't exists");
        }
        File.Delete(path);
    }

    public SaveProfile<T> Load<T>(string profileName) where T : SaveProfileData
    {
        string path = Path.Combine(saveFolder, profileName);
        if (!File.Exists(path))
        {
            throw new Exception($"Save profile {profileName} doesn't exists");
        }

        var fileContents = File.ReadAllText(path);

        return JsonConvert.DeserializeObject<SaveProfile<T>>(fileContents);
    }

    public void Save<T>(SaveProfile<T> save) where T : SaveProfileData
    {
        string path = Path.Combine(saveFolder, save.profileName);
        Debug.Log(path);
        if (File.Exists(path))
        {
            throw new Exception($"Save profile {save.profileName} already exists");
        }

        var jsonString = JsonConvert.SerializeObject(save, Formatting.Indented,
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }
        File.WriteAllText(path, jsonString);
    }

    public void SaveGame()
    {
        OnSaveGame?.Invoke();
    }

    public void LoadGame()
    {
        OnLoadGame?.Invoke();
    }

    public void RegisterSaveable(ISaveable saveable)
    {
        OnSaveGame -= saveable.Save;
        OnLoadGame -= saveable.Load;
        OnSaveGame += saveable.Save;
        OnLoadGame += saveable.Load;
    }
}
