using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadFromCloud : MonoBehaviour
{
    [SerializeField] bool downloadFromCloud = true;
    [SerializeField] float notificationDisplayDuration = 10;
    [SerializeField] Text notificationText;

    private void Start()
    {
#if UNITY_EDITOR
        if (downloadFromCloud)
#endif
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                StartCoroutine(FetchLevels());
            }
            else if (notificationText)
            {
                notificationText.text = "Connect to internet to download the latest content!";
                notificationText.color = new Color(1f, 0.5f, 0, 1);
                StartCoroutine(DisableNotification());
            }
        }
    }

    private IEnumerator FetchLevels()
    {
        using (UnityEngine.Networking.UnityWebRequest webRequest = UnityEngine.Networking.UnityWebRequest.Get("https://drive.google.com/uc?export=download&id=1_OR7r1cZu0V4CzZBzacjLPWJMELIah9y"))
        {
            if (notificationText)
            {
                notificationText.text = "Downloading newest content.";
                notificationText.color = new Color(1f, 0.5f, 0, 1);
                StartCoroutine(DisableNotification());
            }

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.LogWarning("Database Levels Fetch Failed: " + webRequest.error);

                if (notificationText)
                {
                    notificationText.text = "Download Failed";
                    notificationText.color = new Color(1f, 0f, 0, 1);
                    StartCoroutine(DisableNotification());
                }
            }
            else
            {
                try
                {
                    //Debug.Log("Database Fetch Complete!");
                    SaveFileForGoogleDrive.CloudSave cloudSave = JsonUtility.FromJson<SaveFileForGoogleDrive.CloudSave>(SaveSystem.Dencryptor(webRequest.downloadHandler.text));
                    foreach (var levelDataSave in cloudSave.levelDataSaves)
                    {
                        SaveSystem.SaveDataLevel("/Levels/" + levelDataSave.difficulty, levelDataSave.difficulty + levelDataSave.ID.ToString(), levelDataSave);
                    }

                    if (notificationText)
                    {
                        notificationText.text = "Newest content downloaded.";
                        notificationText.color = new Color(0f, 1f, 0, 1);
                        StartCoroutine(DisableNotification());
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning("Database Fetch Complete! With errors! " + e);

                    if (notificationText)
                    {
                        notificationText.text = "Download Failed";
                        notificationText.color = new Color(1f, 0f, 0, 1);
                        StartCoroutine(DisableNotification());
                    }
                }
            }
        }
    }

    private IEnumerator DisableNotification()
    {
        yield return new WaitForSeconds(notificationDisplayDuration);
        notificationText.text = "";
    }
}
