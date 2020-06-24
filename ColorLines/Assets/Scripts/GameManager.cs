using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private LoadLevelSystem LoadLevelSystem;
    private new Camera camera;
    AdvertisementSystem advertisementSystem;
    bool won = false;

    // Start is called before the first frame update
    void Awake()
    {
        advertisementSystem = FindObjectOfType<AdvertisementSystem>();

        camera = Camera.main;

        LoadLevelSystem.LoadLevel();

        foreach (var startPoint in levelData.startPoints)
        {
            LinePathSystem line = new LinePathSystem(startPoint.Value.transform.position, startPoint.Value.outDirection, levelData, startPoint.Value.emitingColor);
            levelData.lines.Add(line);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (won) return;
        if (CheckIfLevelWon()) return;

        //foreach (var line in levelData.lines)
        //{
        //    line.timesCalledThisUpdate = 0;
        //}

        InputHandler();
    }

    private bool CheckIfLevelWon()
    {
        foreach (var endPoint in levelData.endPoints)
        {
            if (!endPoint.Value.hasColor) return false;
        }

        advertisementSystem.AddLevelFinishedPoints(SelectedLevel.levelDataSave.difficulty);
        levelData.victoryPanel.GetComponentInChildren<Text>().text = SelectedLevel.levelDataSave.difficulty.ToString() + " - " + SelectedLevel.levelDataSave.level.ToString() + "\nLevel complete.";
        won = true;
        SaveSystem.SaveDataStatus("/LevelsStatus/" + SelectedLevel.levelDataSave.difficulty, SelectedLevel.levelDataSave.difficulty + SelectedLevel.levelDataSave.ID.ToString(), new LevelStatusSave(LevelStatusSave.Status.Finished));

        bool foundNextLevel = false;
        foreach (var level in SaveSystem.LoadAllDataLevel("/Levels/" + SelectedLevel.levelDataSave.difficulty))
        {
            if (level.level == SelectedLevel.levelDataSave.level + 1)
            {
                SaveSystem.SaveDataStatus("/LevelsStatus/" + level.difficulty, level.difficulty + level.ID.ToString(), new LevelStatusSave(LevelStatusSave.Status.Unlocked));
                SelectedLevel.SelectLevel(level);
                foundNextLevel = true;
                break;
            }
        }
        if (!foundNextLevel)
        {
            foundNextLevel = false;
            foreach (var level in SaveSystem.LoadAllDataLevel("/Levels/" + Enum.GetName(typeof(LevelDataSave.Difficulty), SelectedLevel.levelDataSave.difficulty + 1)))
            {
                if (level.level == 1)
                {
                    SaveSystem.SaveDataStatus("/LevelsStatus/" + level.difficulty, level.difficulty + level.ID.ToString(), new LevelStatusSave(LevelStatusSave.Status.Unlocked));
                    SelectedLevel.SelectLevel(level);
                    foundNextLevel = true;
                    break;
                }
            }
            if (!foundNextLevel)
            {
                levelData.victoryPanel.transform.GetChild(1).Find("Next Button").GetComponent<Button>().interactable = false;
            }
        }

        levelData.victoryPanel.SetActive(true);
        advertisementSystem.ShowAdvertisement();
        return true;
    }

    private bool GetInputPosition(out Vector3 location)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            location = camera.ScreenToWorldPoint(Input.mousePosition);
            location = new Vector3(Mathf.RoundToInt(location.x), Mathf.RoundToInt(location.y), 0);
            return true;
        }

        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                location = camera.ScreenToWorldPoint(touch.position);
                location = new Vector3(Mathf.RoundToInt(location.x), Mathf.RoundToInt(location.y), 0);
                return true;
            }
        }

        location = new Vector3(0, 0, 0);
        return false;
    }

    private void InputHandler()
    {
        Vector3 location = new Vector3();
        if (GetInputPosition(out location))
        {
            if (levelData.interactables.ContainsKey(location))
            {
                levelData.interactables[location].Interact();
            }
        }
    }
}
