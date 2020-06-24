using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdvertisementSystem : MonoBehaviour
{
    float points = 0;
    [SerializeField] float pointsNeedForAd = 600;
    [SerializeField] float pointForFinishingLevel = 60;
    [SerializeField] float pointForDifficulty = 30;

    private void Awake()
    {
        AdvertisementSystem[] objs = FindObjectsOfType<AdvertisementSystem>();

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
#if UNITY_EDITOR
        Advertisement.Initialize("3614604");
#endif
    }

    private void Update()
    {
        AddTimePoints();
    }

    private void AddTimePoints()
    {
        points += Time.deltaTime;
    }

    public void AddLevelFinishedPoints(LevelDataSave.Difficulty difficulty)
    {
        points += pointForFinishingLevel;
        points += pointForDifficulty * (int)difficulty;
    }

    public void ShowAdvertisement()
    {
        if (points >= pointsNeedForAd)
        {
            if (Advertisement.IsReady("video"))
            {
                Advertisement.Show("video");
                points = 0;
            }
        }
    }
}
