using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PConlyObject : MonoBehaviour
{
    private void Awake()
    {
        if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
        {
            this.gameObject.SetActive(false);
        }
    }
}
