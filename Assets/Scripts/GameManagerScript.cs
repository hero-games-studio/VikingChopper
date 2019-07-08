using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField]
    public CameraMovementScript cameraController;

    private static CameraMovementScript cameraMovementScript;

    void Start()
    {
        RenderSettings.fog = false;
        Application.targetFrameRate = 60;
        cameraMovementScript=cameraController;
    }
}
