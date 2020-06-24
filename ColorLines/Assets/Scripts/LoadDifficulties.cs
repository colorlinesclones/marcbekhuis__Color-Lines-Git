using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadDifficulties : MonoBehaviour
{
    [SerializeField] Transform difficultiesButtonParent;
    [SerializeField] GameObject difficultiesButton;
    [SerializeField] Transform difficultiesParent;
    [SerializeField] GameObject difficultiesGameObject;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject difficultySelector;

    List<GameObject> buttons = new List<GameObject>();
    List<GameObject> difficultyTabs = new List<GameObject>();

    private void OnEnable()
    {
        string[] difficulties = Directory.GetDirectories(Application.persistentDataPath + "/Levels/");

        foreach (var difficulty in difficulties)
        {
            string difficultyString = difficulty.Remove(0, (Application.persistentDataPath + "/Levels/").Length);
            GameObject button = Instantiate(difficultiesButton, difficultiesButtonParent);
            button.name = difficultyString + " Button";
            buttons.Add(button);
            GameObject tab = Instantiate(difficultiesGameObject, difficultiesParent);
            tab.name = "Difficulty " + difficultyString;
            tab.GetComponentInChildren<Text>().text = difficultyString;
            difficultyTabs.Add(tab);

            button.GetComponent<Button>().onClick.AddListener(delegate { tab.SetActive(true); difficultySelector.SetActive(false); });
            button.GetComponentInChildren<Text>().text = difficultyString;

            tab.transform.Find("Back Button").GetComponent<Button>().onClick.AddListener(delegate { Destroy(tab); difficultySelector.SetActive(true); difficultyTabs.Remove(tab); });
            tab.transform.Find("Home Button").GetComponent<Button>().onClick.AddListener(delegate { Destroy(tab); mainMenu.SetActive(true); difficultyTabs.Remove(tab); });
        }

        foreach (var button in buttons)
        {
            button.transform.SetSiblingIndex((int)(LevelDataSave.Difficulty)Enum.Parse(typeof(LevelDataSave.Difficulty), button.name.Remove(button.name.Length - " Button".Length)));
        }
    }

    private void OnDisable()
    {
        while (buttons.Count > 0)
        {
            Destroy(buttons[0]);
            buttons.RemoveAt(0);
        }

        int actives = 0;
        while (difficultyTabs.Count > actives)
        {
            if (!difficultyTabs[0].activeSelf)
            {
                Destroy(difficultyTabs[0]);
                difficultyTabs.RemoveAt(0);
            }
            else actives++;
        }
    }
}
