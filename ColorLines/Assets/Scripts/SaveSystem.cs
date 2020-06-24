using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public static class SaveSystem
{

    public static void SaveDataLevel(string saveFolder, string saveName, LevelDataSave saveData)
    {
        if(!Directory.Exists(Application.persistentDataPath + saveFolder))
        {
            Directory.CreateDirectory(Application.persistentDataPath + saveFolder);
        }
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + saveFolder + "/" + saveName + ".txt", Encryptor(json));
    }

    public static void SaveDataStatus(string saveFolder, string saveName, LevelStatusSave saveData)
    {
        if (!Directory.Exists(Application.persistentDataPath + saveFolder))
        {
            Directory.CreateDirectory(Application.persistentDataPath + saveFolder);
        }
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + saveFolder + "/" + saveName + ".txt", Encryptor(json));
    }

    public static LevelDataSave LoadDataLevel(string saveFolder, string saveName)
    {
        if (File.Exists(Application.persistentDataPath + saveFolder + "/" + saveName + ".txt"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + saveFolder + "/" + saveName + ".txt");
            return JsonUtility.FromJson<LevelDataSave>(Dencryptor(json));
        }
        else
        {
            return null;
        }
    }
    public static LevelDataSave[] LoadAllDataLevel(string saveFolder)
    {
        if (!Directory.Exists(Application.persistentDataPath + saveFolder))
        {
            LevelDataSave[] leveldatasave = new LevelDataSave[0];
            return leveldatasave;
        }

        string[] files = Directory.GetFiles(Application.persistentDataPath + saveFolder);
        List<string> levelNames = new List<string>();
        foreach (var item in files)
        {
            if (item.EndsWith(".txt"))
            {
                levelNames.Add(item);
            }
        }

        LevelDataSave[] levelDataSaves = new LevelDataSave[levelNames.Count];

        for (int i = 0; i < levelNames.Count; i++)
        {
            string json = File.ReadAllText(levelNames[i]);
            levelDataSaves[i] = JsonUtility.FromJson<LevelDataSave>(Dencryptor(json));
        }
        return levelDataSaves;
    }

    public static LevelStatusSave LoadDataStatus(string saveFolder, string saveName)
    {
        if (File.Exists(Application.persistentDataPath + saveFolder + "/" + saveName + ".txt"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + saveFolder + "/" + saveName + ".txt");
            return JsonUtility.FromJson<LevelStatusSave>(Dencryptor(json));
        }
        else
        {
            return null;
        }
    }

    public static LevelStatusSave[] LoadAllStatus(string saveFolder)
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath + saveFolder);
        List<string> statusNames = new List<string>();
        foreach (var item in files)
        {
            if (item.EndsWith(".txt"))
            {
                statusNames.Add(item);
            }
        }

        LevelStatusSave[] levelStatusSaves = new LevelStatusSave[statusNames.Count];

        for (int i = 0; i < statusNames.Count; i++)
        {
            string json = File.ReadAllText(statusNames[i]);
            levelStatusSaves[i] = JsonUtility.FromJson<LevelStatusSave>(Dencryptor(json));
        }
        return levelStatusSaves;
    }

    public static string Encryptor(string json)
    {
        string result = "";

        for (int i = 0; i < json.Length; i++)
        {
            result += (char)(json[i] + json.Length - i);
        }

        return result;
    }

    public static string Dencryptor(string json)
    {
        string result = "";

        for (int i = 0; i < json.Length; i++)
        {
            result += (char)(json[i] - json.Length + i);
        }

        return result;
    }
}
