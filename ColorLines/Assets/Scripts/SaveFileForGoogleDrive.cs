using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveFileForGoogleDrive : MonoBehaviour
{
    [Serializable]
    public class CloudSave
    {
        public CloudSave(List<LevelDataSave> levelDataSaves)
        {
            this.levelDataSaves = levelDataSaves;
        }

        public List<LevelDataSave> levelDataSaves;
    }
    List<LevelDataSave> levelDataSaves = new List<LevelDataSave>();

    // Start is called before the first frame update
    void Start()
    {
        SaveAsOne();
    }

    public void SaveAsOne()
    {
        foreach (LevelDataSave.Difficulty difficulty in Enum.GetValues(typeof(LevelDataSave.Difficulty)))
        {
            if (Directory.Exists(Application.persistentDataPath + "/Levels/" + difficulty))
            {
                levelDataSaves.AddRange(SaveSystem.LoadAllDataLevel("/Levels/" + difficulty));
            }
        }

        if (!Directory.Exists(Application.persistentDataPath + "/CloudSave"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/CloudSave");
        }

        string json = JsonUtility.ToJson(new CloudSave(levelDataSaves));
        File.WriteAllText(Application.persistentDataPath + "/CloudSave" + "/" + "ColorLinesLevels" + ".txt", SaveSystem.Encryptor(json));
    }
}
