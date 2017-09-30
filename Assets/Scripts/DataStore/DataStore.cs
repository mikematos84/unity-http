using System.Collections.Generic;

public class DataStore : Singleton<DataStore>
{
    protected DataStore() { }

    private readonly string file = "save.txt";
    private readonly bool encrypt = true;
    private readonly string password = "dcil";
    public bool isLoaded { private set; get; }

    private Dictionary<string, object> saveParams = new Dictionary<string, object>();
    
    public object this[string key]
    {
        get {
            if (saveParams.ContainsKey(key))
            {
                return saveParams[key];
            }
            return null;
        }
        set
        {
            saveParams[key] = value;
        }
    }

    public void Clear()
    {
        saveParams.Clear();
    }

    public string Tag(string tag)
    {
        return file + "?tag=" + tag + "&encrypt=" + encrypt.ToString().ToLower() + "&password=" + password;
    }

    public bool Load()
    {
        if (isLoaded)
            return false;

        if (ES2.Exists(Tag("saveParams")))
        {
            ES2Data info = ES2.LoadAll(Tag("saveParams"));
            saveParams = info.LoadDictionary<string, object>("saveParams");
            isLoaded = true;
            return true;
        }

        isLoaded = false;
        return false;
    }

    public void Save()
    {
        ES2.Save(saveParams, Tag("saveParams"));
    }

    public void Remove(string key)
    {
        saveParams.Remove(key);
    }

    public void Delete()
    {
        ES2.Delete(file);
    }

}
