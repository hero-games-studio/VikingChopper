using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField]
    public CameraMovementScript cameraController;

    private static CameraMovementScript cameraMovementScript;

    [SerializeField]
    private ThrowController throwController;
    public static ThrowController throwControllerScript;

    void Start()
    {
        throwControllerScript = throwController;
        RenderSettings.fog = false;
        Application.targetFrameRate = 60;
        cameraMovementScript = cameraController;
    }

    public static bool checkTapping()
    {
        if (CameraMovementScript.isTapped)
        {
            CameraMovementScript.isTapped = false;
            return true;
        }
        return false;
    }

}
