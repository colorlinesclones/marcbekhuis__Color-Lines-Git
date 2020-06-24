using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAds : MonoBehaviour
{
    AdvertisementSystem advertisementSystem;

    // Start is called before the first frame update
    void Start()
    {
        advertisementSystem = FindObjectOfType<AdvertisementSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        advertisementSystem.ShowAdvertisement();
    }
}
