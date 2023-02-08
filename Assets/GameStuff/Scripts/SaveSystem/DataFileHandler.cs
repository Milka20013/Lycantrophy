using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DataFileHandler
{
    private readonly string dataDirectoryPath = "";
    private readonly string dataFileName = "";

    private readonly bool useEncryption = false;
    private readonly string encryptionWord = "Miskolc";

    public DataFileHandler(string dataDirectoryPath, string dataFileName, bool useEncryption)
    {
        this.dataDirectoryPath = dataDirectoryPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader str = new StreamReader(fileStream))
                    {
                        dataToLoad = str.ReadToEnd();
                    }
                }

                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception)
            {
                Debug.LogError("Couldn't load data");
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream fileStream = new FileStream(fullPath,FileMode.Create)) 
            {
                using (StreamWriter str = new StreamWriter(fileStream))
                {
                    str.Write(dataToStore);
                }
            }
        }
        catch (Exception)
        {
            Debug.LogError("Couldn't save the data");
        }
    }

    public string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionWord[i % encryptionWord.Length]);
        }
        return modifiedData;
    }
}
