using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class StandaloneSaveManager 
{

    public static void BinarySerialize(string filename, object data)
    {
        FileStream file = File.Open(GetFullPath(filename), FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();

        try
        {
            bf.Serialize(file, data);
        }
        catch (SerializationException e)
        {
            Debug.Log("BinarySerialize failed: " + e.Message);
        }
        finally
        {
            file.Close();
        }
    }

    public static T BinaryDeserialize<T>(string filename) where T : new()
    {
        T result = new T();

        if (!FileExists(filename))
        {
            return result;
        }

        FileStream file = File.Open(GetFullPath(filename), FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();

        try
        {
            result = (T)bf.Deserialize(file);
        }
        catch (SerializationException e)
        {
            Debug.Log("BinaryDeserialize failed: " + e.Message);
        }
        finally
        {
            file.Close();
        }
        return result;
    }

    public static bool FileExists(string filename)
    {
        return File.Exists(GetFullPath(filename));
    }

    public static void Delete(string filename)
    {
        File.Delete(GetFullPath(filename));
    }

    public static string GetFullPath(string filename)
    {
        return Path.Combine(Application.persistentDataPath, filename);
    }

    public static void SaveList<T>(List<T> list, string listName) where T : ScriptableObject
    {
        List<int> GUIDs = new List<int>();
        foreach (T item in list)
        {
            GUIDs.Add(ToGUID<T>(item));
        }

        BinarySerialize(GetFileName(listName), GUIDs);
    }

    public static List<T> LoadList<T>(string listName) where T: ScriptableObject
    {
        List<T> result = new List<T>();
        List<int> GUIDs = BinaryDeserialize<List<int>>(GetFileName(listName));

        foreach (int id in GUIDs)
        {
            result.Add(FromGUID<T>(id));
        }

        return result;
    }

    public static string GetFileName(string listName)
    {
        return listName + ".list";
    }

    private static int ToGUID<T>(T item) where T : ScriptableObject
    {
        int id = SaveSystem.current.Database.GetId<T>(item);

        return id;
    }

    private static T FromGUID<T>(int id) where T : ScriptableObject
    {
        Debug.Log("Load " + id);
        T item = (T) SaveSystem.current.Database.GetById<T>(id);

        return item;
    }
}
