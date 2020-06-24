using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevels : MonoBehaviour
{
    [SerializeField] GameObject levelButton;
    [SerializeField] Sprite lockSprite;
    [SerializeField] Sprite checkSprite;
    Transform levels;

    private void OnEnable()
    {
        levels = this.transform.Find("Levels");
        string difficulty = this.name.Remove(0, "Difficulty ".Length);
        foreach (var levelDataSave in SaveSystem.LoadAllDataLevel("/Levels/" + difficulty))
        {
            GameObject level = Instantiate(levelButton, levels);
            level.name = "Level " + levelDataSave.level.ToString();
            level.GetComponentInChildren<Text>().text = levelDataSave.level.ToString();
            level.GetComponent<Button>().onClick.AddListener(delegate { SelectedLevel.SelectLevel(levelDataSave); });

            if (File.Exists(Application.persistentDataPath + "/LevelsStatus/" + difficulty + "/" + difficulty + levelDataSave.ID.ToString() + ".txt"))
            {
                LevelStatusSave levelStatusSave = SaveSystem.LoadDataStatus("/LevelsStatus/" + difficulty, difficulty + levelDataSave.ID.ToString());
                SetImage(level,levelStatusSave.status);
            }
            else
            {
                SaveSystem.SaveDataStatus("/LevelsStatus/" + difficulty, difficulty + levelDataSave.ID.ToString(), new LevelStatusSave(levelDataSave.beginStatus));
                SetImage(level, levelDataSave.beginStatus);
            }
        }
    }

    private void SetImage(GameObject level, LevelStatusSave.Status status)
    {
        switch (status)
        {
            case LevelStatusSave.Status.Locked:
                {
                    level.GetComponent<Button>().interactable = false;
                    Image image = level.transform.Find("Image").GetComponent<Image>();
                    image.color = new Color(0, 0, 0, 1);
                    image.sprite = lockSprite;
                    break;
                }
            case LevelStatusSave.Status.Unlocked:
                {
                    level.transform.Find("Image").GetComponent<Image>().gameObject.SetActive(false);
                    break;
                }
            case LevelStatusSave.Status.Finished:
                {
                    Image image = level.transform.Find("Image").GetComponent<Image>();
                    image.color = new Color(0, 1, 0, 1);
                    image.sprite = checkSprite;
                    break;
                }
            default:
                break;
        }
    }
}
