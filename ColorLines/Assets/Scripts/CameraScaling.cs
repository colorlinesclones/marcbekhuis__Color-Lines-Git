using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraScaling : MonoBehaviour
{

    // Set this to the in-world distance between the left & right edges of your scene.
    public float sceneWidth = 10;
    public float sceneHeight = 10;

    Camera camera;
    void Start()
    {
        camera = GetComponent<Camera>();
#if !UNITY_EDITOR
        ChangeCameraSize();
#endif
    }

    // Adjust the camera's height so the desired scene width fits in view
    // even if the screen/window size changes dynamically.
#if UNITY_EDITOR
    void Update()
    {
        ChangeCameraSize();
    }
#endif

    private void ChangeCameraSize()
    {
        if (Application.platform != RuntimePlatform.WindowsPlayer)
        {
            float unitsPerPixelWidth = sceneWidth / Screen.width;

            float desiredHalfHeight = 0.5f * unitsPerPixelWidth * Screen.height;

            float unitsPerPixelHeight = sceneHeight / Screen.height;

            float desiredHalfWidth = 0.5f * unitsPerPixelHeight * Screen.width;

            if (desiredHalfHeight > desiredHalfWidth)
            {
                camera.orthographicSize = desiredHalfHeight;
            }
            else
            {
                camera.orthographicSize = desiredHalfWidth;
            }
        }
        else
        {
            camera.orthographicSize = 10;
        }
    }
}
