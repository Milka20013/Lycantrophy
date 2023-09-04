using System;
[Serializable]
public class SaveProfile<T> where T : SaveProfileData
{
    private static readonly string extension = ".gamec";
    public string profileName;
    public T saveData;

    private SaveProfile() { }

    public SaveProfile(string name, T saveData)
    {
        this.profileName = name + extension;
        this.saveData = saveData;
    }
}

public abstract record SaveProfileData { }
